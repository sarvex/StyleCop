﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadabilityRules.cs" company="http://stylecop.codeplex.com">
//   MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. If you cannot locate the  
//   Microsoft Public License, please send an email to dlr@microsoft.com. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
// <summary>
//   Readability rules.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
extern alias JB;

namespace StyleCop.ReSharper513.CodeCleanup.Rules
{
    #region Using Directives

    using System.Collections.Generic;

    using JetBrains.Application;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CodeStyle;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
    using JetBrains.ReSharper.Psi.CSharp.Parsing;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.CSharp.Tree.Extensions;
    using JetBrains.ReSharper.Psi.ExtensionsAPI;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
    using JetBrains.ReSharper.Psi.Tree;

    using StyleCop.ReSharper513.CodeCleanup.Options;
    using StyleCop.ReSharper513.Core;
    using StyleCop.ReSharper513.Diagnostics;
    using StyleCop.ReSharper513.Extensions;

    #endregion

    /// <summary>
    /// Readability rules.
    /// </summary>
    internal class ReadabilityRules
    {
        #region Static Fields

        /// <summary>
        /// The built-in type aliases for C#.
        /// </summary>
        private static readonly string[][] BuiltInTypes = new[]
                                                              {
                                                                  new[] { "Boolean", "System.Boolean", "bool" }, new[] { "Object", "System.Object", "object" }, 
                                                                  new[] { "String", "System.String", "string" }, new[] { "Int16", "System.Int16", "short" }, 
                                                                  new[] { "UInt16", "System.UInt16", "ushort" }, new[] { "Int32", "System.Int32", "int" }, 
                                                                  new[] { "UInt32", "System.UInt32", "uint" }, new[] { "Int64", "System.Int64", "long" }, 
                                                                  new[] { "UInt64", "System.UInt64", "ulong" }, new[] { "Double", "System.Double", "double" }, 
                                                                  new[] { "Single", "System.Single", "float" }, new[] { "Byte", "System.Byte", "byte" }, 
                                                                  new[] { "SByte", "System.SByte", "sbyte" }, new[] { "Char", "System.Char", "char" }, 
                                                                  new[] { "Decimal", "System.Decimal", "decimal" }
                                                              };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Moves the comment token specified after the next available non whitespace char (normally an open curly bracket).
        /// </summary>
        /// <param name="commentTokenNode">
        /// The comment token to move.
        /// </param>
        public static void MoveCommentInsideNextOpenCurlyBracket(ITokenNode commentTokenNode)
        {
            using (WriteLockCookie.Create(true))
            {
                // move comment inside block curly bracket here
                // we copy it, then insert it and then delete the copied one
                ITokenNode startOfTokensToDelete = Utils.GetFirstNonWhitespaceTokenToLeft(commentTokenNode).GetNextToken();
                ITokenNode endOfTokensToDelete = Utils.GetFirstNewLineTokenToRight(commentTokenNode);
                ITokenNode startOfTokensToFormat = startOfTokensToDelete.GetPrevToken();

                ITokenNode openCurlyBracketTokenNode = Utils.GetFirstNonWhitespaceTokenToRight(commentTokenNode);
                ITokenNode newCommentTokenNode = commentTokenNode.CopyElement(null);
                ITokenNode tokenNodeToInsertAfter = Utils.GetFirstNewLineTokenToRight(openCurlyBracketTokenNode);
                LowLevelModificationUtil.AddChildAfter(tokenNodeToInsertAfter, new[] { newCommentTokenNode });
                LowLevelModificationUtil.AddChildAfter(newCommentTokenNode, newCommentTokenNode.InsertNewLineAfter());

                DeleteChildRange(startOfTokensToDelete, endOfTokensToDelete);
                ITokenNode endOfTokensToFormat = newCommentTokenNode;

                CSharpFormatterHelper.FormatterInstance.Format(startOfTokensToFormat, endOfTokensToFormat);
            }
        }

        /// <summary>
        /// Moves the IStartRegion specified inside the next open curly bracket and moves the corresponding end region inside too.
        /// </summary>
        /// <param name="startRegionNode">
        /// The node to move.
        /// </param>
        public static void MoveRegionInsideNextOpenCurlyBracket(IStartRegionNode startRegionNode)
        {
            using (WriteLockCookie.Create(true))
            {
                ITokenNode newLocationTokenNode = Utils.GetFirstNonWhitespaceTokenToRight(startRegionNode.Message);

                // if its a start region there is probably a corresponding end region
                // find it, and move it inside the block
                // find the position to delete from
                ITokenNode startOfTokensToDelete = Utils.GetFirstNewLineTokenToLeft(startRegionNode.NumberSign);
                ITokenNode endOfTokensToDelete = Utils.GetFirstNewLineTokenToRight(startRegionNode.Message);
                ITokenNode startOfTokensToFormat = startOfTokensToDelete.GetPrevToken();

                IEndRegionNode endRegionNode = startRegionNode.EndRegion;
                IStartRegionNode newStartRegionNode = startRegionNode.CopyElement(null);
                ITokenNode firstNonWhitespaceAfterBracket = Utils.GetFirstNonWhitespaceTokenToRight(newLocationTokenNode);

                LowLevelModificationUtil.AddChildBefore(firstNonWhitespaceAfterBracket, new[] { newStartRegionNode });

                newStartRegionNode.ToTreeNode().InsertNewLineAfter();

                LowLevelModificationUtil.DeleteChildRange(startOfTokensToDelete, endOfTokensToDelete);
                IElement endOfTokensToFormat = (IElement)newStartRegionNode;

                if (endRegionNode != null)
                {
                    startOfTokensToDelete = Utils.GetFirstNewLineTokenToLeft(endRegionNode.NumberSign);
                    endOfTokensToDelete = Utils.GetFirstNewLineTokenToRight(endRegionNode.NumberSign);

                    IEndRegionNode newEndRegionNode = endRegionNode.CopyElement(null);
                    ITokenNode newLineToken = Utils.GetFirstNonWhitespaceTokenToLeft(endRegionNode.NumberSign);
                    LowLevelModificationUtil.AddChildBefore(newLineToken, new[] { newEndRegionNode });

                    newEndRegionNode.InsertNewLineAfter();

                    LowLevelModificationUtil.DeleteChildRange(startOfTokensToDelete, endOfTokensToDelete);
                    endOfTokensToFormat = newLineToken;
                }

                CSharpFormatterHelper.FormatterInstance.Format(startOfTokensToFormat, endOfTokensToFormat);
            }
        }

        /// <summary>
        /// Remove empty comments.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        public static void RemoveEmptyComments(ITreeNode node)
        {
            // we don't remove empty lines from Element Doc Comments in here
            // the DeclarationHeader types take care of that.
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ICommentNode commentNode = currentNode as ICommentNode;
                    if (commentNode != null && !(commentNode is IDocCommentNode))
                    {
                        if (commentNode.CommentText.Trim() == string.Empty)
                        {
                            ITokenNode leftToken = Utils.GetFirstNewLineTokenToLeft((ITokenNode)currentNode);

                            ITokenNode rightToken = Utils.GetFirstNewLineTokenToRight((ITokenNode)currentNode);

                            if (leftToken == null)
                            {
                                leftToken = (ITokenNode)currentNode;
                            }
                            else
                            {
                                leftToken = leftToken.GetNextToken();
                            }

                            if (rightToken == null)
                            {
                                rightToken = (ITokenNode)currentNode;
                            }
                            else
                            {
                                currentNode = rightToken.GetNextToken();
                            }

                            using (WriteLockCookie.Create(true))
                            {
                                LowLevelModificationUtil.DeleteChildRange(leftToken, rightToken);
                            }
                        }
                    }
                }

                if (currentNode != null && currentNode.FirstChild != null)
                {
                    RemoveEmptyComments(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Swap base to this unless local implementation.
        /// </summary>
        /// <param name="invocationExpression">
        /// The invocation expression.
        /// </param>
        public static void SwapBaseToThisUnlessLocalImplementation(IInvocationExpression invocationExpression)
        {
            bool isOverride = false;

            bool isNew = false;

            ICSharpExpression invokedExpression = invocationExpression.InvokedExpression;

            if (invokedExpression != null)
            {
                IReferenceExpressionNode referenceExpressionNode = invokedExpression as IReferenceExpressionNode;

                if (referenceExpressionNode != null)
                {
                    IReferenceExpression referenceExpression = invokedExpression as IReferenceExpression;
                    if (referenceExpression != null)
                    {
                        ICSharpExpression qualifierExpression = referenceExpression.QualifierExpression;
                        if (qualifierExpression is IBaseExpression)
                        {
                            string methodName = referenceExpressionNode.NameIdentifier.Name;

                            ICSharpTypeDeclaration typeDeclaration = invocationExpression.GetContainingElement<ICSharpTypeDeclaration>(true);

                            if (typeDeclaration != null)
                            {
                                foreach (ICSharpTypeMemberDeclaration memberDeclaration in typeDeclaration.MemberDeclarations)
                                {
                                    if (memberDeclaration.DeclaredName == methodName)
                                    {
                                        IMethodDeclaration methodDeclaration = memberDeclaration as IMethodDeclaration;
                                        if (methodDeclaration != null)
                                        {
                                            isOverride = methodDeclaration.IsOverride;
                                            isNew = methodDeclaration.IsNew();
                                            break;
                                        }
                                    }
                                }

                                if (isOverride || isNew)
                                {
                                    return;
                                }

                                using (WriteLockCookie.Create(true))
                                {
                                    // swap the base to this
                                    ICSharpExpression expression = CSharpElementFactory.GetInstance(invocationExpression.GetPsiModule()).CreateExpression("this");

                                    referenceExpression.SetQualifierExpression(expression);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swap to built in type alias.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        public static void SwapToBuiltInTypeAlias(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                ITypeArgumentListNode typeArgumentListNode = currentNode as ITypeArgumentListNode;
                if (typeArgumentListNode != null)
                {
                    SwapGenericDeclarationToBuiltInType(typeArgumentListNode);
                }
                else
                {
                    IMethodDeclarationNode methodDeclarationNode = currentNode as IMethodDeclarationNode;
                    if (methodDeclarationNode != null)
                    {
                        SwapReturnTypeToBuiltInType(methodDeclarationNode);
                    }
                    else
                    {
                        IVariableDeclarationNode variableDeclaration = currentNode as IVariableDeclarationNode;
                        if (variableDeclaration != null)
                        {
                            SwapVariableDeclarationToBuiltInType(variableDeclaration);
                        }
                        else
                        {
                            IObjectCreationExpressionNode creationExpressionNode = currentNode as IObjectCreationExpressionNode;
                            if (creationExpressionNode != null)
                            {
                                SwapObjectCreationToBuiltInType(creationExpressionNode);
                            }
                            else
                            {
                                IArrayCreationExpressionNode arrayCreationNode = currentNode as IArrayCreationExpressionNode;
                                if (arrayCreationNode != null)
                                {
                                    SwapArrayCreationToBuiltInType(arrayCreationNode);
                                }
                                else
                                {
                                    IReferenceExpressionNode referenceExpressionNode = currentNode as IReferenceExpressionNode;
                                    if (referenceExpressionNode != null)
                                    {
                                        SwapReferenceExpressionToBuiltInType(referenceExpressionNode);
                                    }
                                }
                            }
                        }
                    }
                }

                if (currentNode != null && currentNode.FirstChild != null)
                {
                    SwapToBuiltInTypeAlias(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Executes the cleanup rules.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <param name="file">
        /// The file to process.
        /// </param>
        public void Execute(ReadabilityOptions options, ICSharpFile file)
        {
            StyleCopTrace.In(options, file);
            bool dontPrefixCallsWithBaseUnlessLocalImplementationExists = options.SA1100DoNotPrefixCallsWithBaseUnlessLocalImplementationExists;
            bool codeMustNotContainEmptyStatements = options.SA1106CodeMustNotContainEmptyStatements;
            bool blockStatementsMustNotContainEmbeddedComments = options.SA1108BlockStatementsMustNotContainEmbeddedComments;
            bool blockStatementsMustNotContainEmbeddedRegions = options.SA1109BlockStatementsMustNotContainEmbeddedRegions;
            bool commentsMustContainText = options.SA1120CommentsMustContainText;
            bool useBuiltInTypeAlias = options.SA1121UseBuiltInTypeAlias;
            bool useStringEmptyForEmptyStrings = options.SA1122UseStringEmptyForEmptyStrings;
            bool dontPlaceRegionsWithinElements = options.SA1123DoNotPlaceRegionsWithinElements;
            bool codeMustNotContainEmptyRegions = options.SA1124CodeMustNotContainEmptyRegions;

            if (dontPlaceRegionsWithinElements)
            {
                this.DoNotPlaceRegionsWithinElements(file.ToTreeNode().FirstChild);
            }

            if (blockStatementsMustNotContainEmbeddedComments)
            {
                this.BlockStatementsMustNotContainEmbeddedComments(file.ToTreeNode().FirstChild);
            }

            if (blockStatementsMustNotContainEmbeddedRegions)
            {
                this.BlockStatementsMustNotContainEmbeddedRegions(file.ToTreeNode().FirstChild);
            }

            if (codeMustNotContainEmptyStatements)
            {
                this.CodeMustNotContainEmptyStatements(file.ToTreeNode().FirstChild);
            }

            if (codeMustNotContainEmptyRegions)
            {
                this.CodeMustNotContainEmptyRegions(file.ToTreeNode().FirstChild);
            }

            if (useStringEmptyForEmptyStrings)
            {
                this.ReplaceEmptyStringsWithStringDotEmpty(file.ToTreeNode().FirstChild);
            }

            if (useBuiltInTypeAlias)
            {
                SwapToBuiltInTypeAlias(file.ToTreeNode().FirstChild);
            }

            if (commentsMustContainText)
            {
                RemoveEmptyComments(file.ToTreeNode().FirstChild);
            }

            if (dontPrefixCallsWithBaseUnlessLocalImplementationExists)
            {
                this.DoNotPrefixCallsWithBaseUnlessLocalImplementationExists(file.ToTreeNode().FirstChild);
            }

            StyleCopTrace.Out();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete child range.
        /// </summary>
        /// <param name="first">
        /// The first token to delete.
        /// </param>
        /// <param name="last">
        /// The last token to delete.
        /// </param>
        private static void DeleteChildRange(ITokenNode first, ITokenNode last)
        {
            using (WriteLockCookie.Create(true))
            {
                List<ITokenNode> a = new List<ITokenNode>();

                ITokenNode tokenNodeToStopAt = last.GetNextToken();
                for (ITokenNode foundToken = first; foundToken != tokenNodeToStopAt; foundToken = foundToken.GetNextToken())
                {
                    a.Add(foundToken);
                }

                foreach (ITokenNode tokenNode in a)
                {
                    LowLevelModificationUtil.DeleteChild(tokenNode);
                }
            }
        }

        /// <summary>
        /// Process for each variable declaration.
        /// </summary>
        /// <param name="foreachVariableDeclaration">
        /// The for each variable declaration.
        /// </param>
        private static void ProcessForeachVariableDeclaration(IForeachVariableDeclaration foreachVariableDeclaration)
        {
            ILocalVariable variable = (ILocalVariable)foreachVariableDeclaration.DeclaredElement;
            if (variable != null)
            {
                if (!foreachVariableDeclaration.IsVar)
                {
                    using (WriteLockCookie.Create(true))
                    {
                        foreachVariableDeclaration.SetType(variable.Type);
                    }
                }
            }
        }

        /// <summary>
        /// Process local variable declaration.
        /// </summary>
        /// <param name="localVariableDeclaration">
        /// The local variable declaration.
        /// </param>
        private static void ProcessLocalVariableDeclaration(ILocalVariableDeclarationNode localVariableDeclaration)
        {
            IMultipleLocalVariableDeclarationNode multipleDeclaration = MultipleLocalVariableDeclarationNodeNavigator.GetByDeclarator(localVariableDeclaration);

            if (multipleDeclaration.Declarators.Count > 1)
            {
                IType newType = CSharpTypeFactory.CreateType(multipleDeclaration.TypeUsage);

                using (WriteLockCookie.Create(true))
                {
                    multipleDeclaration.SetTypeUsage(CSharpElementFactory.GetInstance(localVariableDeclaration.GetPsiModule()).CreateTypeUsageNode(newType));
                }
            }
            else
            {
                ILocalVariable variable = (ILocalVariable)localVariableDeclaration.DeclaredElement;
                if (variable != null)
                {
                    if (!multipleDeclaration.IsVar)
                    {
                        using (WriteLockCookie.Create(true))
                        {
                            localVariableDeclaration.SetType(variable.Type);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swap array creation to built in type.
        /// </summary>
        /// <param name="arrayCreationExpression">
        /// The array creation expression.
        /// </param>
        private static void SwapArrayCreationToBuiltInType(IArrayCreationExpression arrayCreationExpression)
        {
            if (!arrayCreationExpression.IsImplicitlyTypedArray)
            {
                using (WriteLockCookie.Create(true))
                {
                    bool fileIsCSharp30 = Utils.IsCSharp30(arrayCreationExpression.GetProjectFile());

                    // If the array creation type is the same type as the initializer (and we are CSharp 3.0 or greater) remove it completely
                    IArrayType arrayType = arrayCreationExpression.Type() as IArrayType;
                    if ((arrayType != null) && arrayCreationExpression.ArrayInitializer != null && fileIsCSharp30
                        && arrayType.ElementType.Equals(arrayCreationExpression.ArrayInitializer.GetElementType(true)))
                    {
                        IList<IRankSpecifierNode> dims = arrayCreationExpression.ToTreeNode().Dims;
                        arrayCreationExpression.SetArrayType(null);
                        for (int i = 0; i < (dims.Count - 1); i++)
                        {
                            using (WriteLockCookie.Create(true))
                            {
                                ModificationUtil.DeleteChild(dims[i]);
                            }
                        }

                        foreach (ICSharpExpression size in arrayCreationExpression.Sizes)
                        {
                            if (size != null)
                            {
                                using (WriteLockCookie.Create(true))
                                {
                                    ModificationUtil.DeleteChild(size.ToTreeNode());
                                }
                            }
                        }
                    }
                    else
                    {
                        using (WriteLockCookie.Create(true))
                        {
                            arrayCreationExpression.SetArrayType(arrayType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swap generic declaration to built in type.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private static void SwapGenericDeclarationToBuiltInType(ITypeArgumentListNode node)
        {
            IPsiModule project = node.GetPsiModule();
            IList<ITypeUsageNode> typeUsageNodes = node.TypeArgumentNodes;
            IList<IType> types = node.TypeArguments;

            using (WriteLockCookie.Create(true))
            {
                for (int i = 0; i < typeUsageNodes.Count; i++)
                {
                    if (!types[i].IsUnknown)
                    {
                        ITypeUsageNode newTypeUsageNode = CSharpElementFactory.GetInstance(project).CreateTypeUsageNode(types[i]);

                        using (WriteLockCookie.Create(true))
                        {
                            ModificationUtil.ReplaceChild(typeUsageNodes[i], newTypeUsageNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swap object creation to built in type.
        /// </summary>
        /// <param name="objectCreationExpressionNode">
        /// The object creation expression node.
        /// </param>
        private static void SwapObjectCreationToBuiltInType(IObjectCreationExpressionNode objectCreationExpressionNode)
        {
            IPsiModule project = objectCreationExpressionNode.GetPsiModule();

            using (WriteLockCookie.Create(true))
            {
                IObjectCreationExpressionNode tmpExpression =
                    (IObjectCreationExpressionNode)
                    CSharpElementFactory.GetInstance(project).CreateExpression("new $0?()", new object[] { objectCreationExpressionNode.Type() });
                if (tmpExpression != null)
                {
                    objectCreationExpressionNode.SetCreatedTypeUsage(tmpExpression.CreatedTypeUsage);
                }
            }
        }

        /// <summary>
        /// Swap reference expression to built in type.
        /// </summary>
        /// <param name="referenceExpression">
        /// The reference expression.
        /// </param>
        private static void SwapReferenceExpressionToBuiltInType(IReferenceExpression referenceExpression)
        {
            IPsiModule project = referenceExpression.GetPsiModule();
            ICSharpExpression qualifierExpression = referenceExpression.QualifierExpression;

            if (qualifierExpression != null)
            {
                using (WriteLockCookie.Create(true))
                {
                    foreach (string[] builtInType in BuiltInTypes)
                    {
                        string text = qualifierExpression.GetText();
                        if (text == builtInType[0] || text == builtInType[1])
                        {
                            ICSharpExpression expression = CSharpElementFactory.GetInstance(project).CreateExpression(builtInType[2], new object[0]);
                            referenceExpression.SetQualifierExpression(expression);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swap return type to built in type.
        /// </summary>
        /// <param name="methodDeclaration">
        /// The method declaration.
        /// </param>
        private static void SwapReturnTypeToBuiltInType(IMethodDeclaration methodDeclaration)
        {
            using (WriteLockCookie.Create(true))
            {
                methodDeclaration.SetType(methodDeclaration.GetReturnType());
            }
        }

        /// <summary>
        /// Swap variable declaration to built in type.
        /// </summary>
        /// <param name="variableDeclaration">
        /// The variable declaration.
        /// </param>
        private static void SwapVariableDeclarationToBuiltInType(IVariableDeclaration variableDeclaration)
        {
            if (variableDeclaration is ILocalVariableDeclarationNode)
            {
                ProcessLocalVariableDeclaration((ILocalVariableDeclarationNode)variableDeclaration);
            }
            else if (variableDeclaration is IForeachVariableDeclarationNode)
            {
                ProcessForeachVariableDeclaration((IForeachVariableDeclarationNode)variableDeclaration);
            }
            else
            {
                IDeclaredElement declaredElement = variableDeclaration.DeclaredElement;
                ITypeOwner typeOwner = (ITypeOwner)declaredElement;

                if (typeOwner != null)
                {
                    using (WriteLockCookie.Create(true))
                    {
                        variableDeclaration.SetType(typeOwner.Type);
                    }
                }
            }
        }

        /// <summary>
        /// Block statements must not contain embedded comments.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void BlockStatementsMustNotContainEmbeddedComments(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.LBRACE)
                    {
                        ITokenNode previousTokenNode = Utils.GetFirstNonWhitespaceTokenToLeft(tokenNode);
                        if (previousTokenNode.GetTokenType() == CSharpTokenType.END_OF_LINE_COMMENT)
                        {
                            ICommentNode commentNode = previousTokenNode.GetContainingElement<ICommentNode>(true);
                            MoveCommentInsideNextOpenCurlyBracket(commentNode);
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.BlockStatementsMustNotContainEmbeddedComments(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Block statements must not contain embedded regions.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void BlockStatementsMustNotContainEmbeddedRegions(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.LBRACE)
                    {
                        ITokenNode previousTokenNode = Utils.GetFirstNonWhitespaceTokenToLeft(tokenNode);
                        ITokenNode previousTokenNode2 = previousTokenNode.GetPrevToken();

                        if (previousTokenNode.GetTokenType() == CSharpTokenType.PP_MESSAGE && previousTokenNode2.GetTokenType() == CSharpTokenType.PP_START_REGION)
                        {
                            IStartRegionNode startRegionNode = previousTokenNode.GetContainingElement<IStartRegionNode>(true);
                            if (startRegionNode != null)
                            {
                                MoveRegionInsideNextOpenCurlyBracket(startRegionNode);
                            }
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.BlockStatementsMustNotContainEmbeddedRegions(currentNode.FirstChild);
                }
            }
        }

        private void CodeMustNotContainEmptyRegions(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.PP_START_REGION)
                    {
                        IStartRegionNode startRegionNode = tokenNode.GetContainingElement<IStartRegionNode>(true);

                        IEndRegionNode endRegion = startRegionNode.EndRegion;

                        if (endRegion != null)
                        {
                            ITokenNode previousTokenNode = Utils.GetFirstNonWhitespaceTokenToLeft(endRegion.NumberSign);

                            IStartRegionNode a = previousTokenNode.GetContainingElement<IStartRegionNode>(true);

                            if (a != null && a.Equals(startRegionNode))
                            {
                                using (WriteLockCookie.Create(true))
                                {
                                    ModificationUtil.DeleteChild(startRegionNode);
                                    ModificationUtil.DeleteChild(endRegion);
                                }
                            }
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.CodeMustNotContainEmptyRegions(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Code must not contain empty statements.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void CodeMustNotContainEmptyStatements(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.SEMICOLON && !(tokenNode.Parent is IForStatement))
                    {
                        ITokenNode nextNonWhitespaceToken = Utils.GetFirstNonWhitespaceTokenToRight(tokenNode);

                        while (nextNonWhitespaceToken.GetTokenType() == CSharpTokenType.SEMICOLON)
                        {
                            using (WriteLockCookie.Create(true))
                            {
                                if (nextNonWhitespaceToken.GetNextToken().GetTokenType() == CSharpTokenType.WHITE_SPACE)
                                {
                                    LowLevelModificationUtil.DeleteChild(nextNonWhitespaceToken.GetNextToken());
                                }

                                // remove the spare semi colon
                                LowLevelModificationUtil.DeleteChild(nextNonWhitespaceToken);
                                nextNonWhitespaceToken = Utils.GetFirstNonWhitespaceTokenToRight(tokenNode);
                            }
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.CodeMustNotContainEmptyStatements(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Do not place regions within elements.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void DoNotPlaceRegionsWithinElements(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.PP_START_REGION)
                    {
                        IStartRegionNode startRegionNode = tokenNode.GetContainingElement<IStartRegionNode>(true);
                        if (startRegionNode != null)
                        {
                            if (startRegionNode.Parent is IBlock)
                            {
                                // we're in a block so remove the end and start region
                                IEndRegionNode endRegionNode = startRegionNode.EndRegion;

                                using (WriteLockCookie.Create(true))
                                {
                                    LowLevelModificationUtil.DeleteChild(endRegionNode);
                                    LowLevelModificationUtil.DeleteChild(startRegionNode);
                                }
                            }
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.DoNotPlaceRegionsWithinElements(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Do not prefix calls with base unless local implementation exists.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void DoNotPrefixCallsWithBaseUnlessLocalImplementationExists(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                IInvocationExpression invocationExpression = currentNode as IInvocationExpression;
                if (invocationExpression != null)
                {
                    SwapBaseToThisUnlessLocalImplementation(invocationExpression);
                }

                if (currentNode.FirstChild != null)
                {
                    this.DoNotPrefixCallsWithBaseUnlessLocalImplementationExists(currentNode.FirstChild);
                }
            }
        }

        /// <summary>
        /// Replace empty strings with string dot empty.
        /// </summary>
        /// <param name="node">
        /// The node to process.
        /// </param>
        private void ReplaceEmptyStringsWithStringDotEmpty(ITreeNode node)
        {
            for (ITreeNode currentNode = node; currentNode != null; currentNode = currentNode.NextSibling)
            {
                if (currentNode is ITokenNode)
                {
                    ITokenNode tokenNode = currentNode as ITokenNode;

                    if (tokenNode.GetTokenType() == CSharpTokenType.STRING_LITERAL)
                    {
                        IAttribute attribute = tokenNode.GetContainingElement<IAttribute>(true);
                        ISwitchLabelStatement switchLabelStatement = tokenNode.GetContainingElement<ISwitchLabelStatement>(true);
                        IConstantDeclaration constantDeclaration = tokenNode.GetContainingElement<IConstantDeclaration>(true);

                        // Not for attributes switch labels or constant declarations
                        if (attribute == null && switchLabelStatement == null && constantDeclaration == null)
                        {
                            string text = currentNode.GetText();
                            if (text == "\"\"" || text == "@\"\"")
                            {
                                const string NewText = "string.Empty";
                                ITokenNode newLiteral =
                                    (ITokenNode)
                                    CSharpTokenType.STRING_LITERAL.Create(new JB::JetBrains.Text.StringBuffer(NewText), new TreeOffset(0), new TreeOffset(NewText.Length));

                                using (WriteLockCookie.Create(true))
                                {
                                    LowLevelModificationUtil.ReplaceChildRange(currentNode, currentNode, new ITreeNode[] { newLiteral });
                                }
                            }
                        }
                    }
                }

                if (currentNode.FirstChild != null)
                {
                    this.ReplaceEmptyStringsWithStringDotEmpty(currentNode.FirstChild);
                }
            }
        }

        #endregion
    }
}