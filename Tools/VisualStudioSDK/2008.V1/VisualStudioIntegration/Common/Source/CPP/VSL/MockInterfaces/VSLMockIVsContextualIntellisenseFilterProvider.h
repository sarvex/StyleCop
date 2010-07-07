/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#ifndef IVSCONTEXTUALINTELLISENSEFILTERPROVIDER_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
#define IVSCONTEXTUALINTELLISENSEFILTERPROVIDER_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5

#if _MSC_VER > 1000
#pragma once
#endif

#include "vsshell80.h"

#pragma warning(push)
#pragma warning(disable : 4510) // default constructor could not be generated
#pragma warning(disable : 4610) // can never be instantiated - user defined constructor required
#pragma warning(disable : 4512) // assignment operator could not be generated
#pragma warning(disable : 6011) // Dereferencing NULL pointer (a NULL derference is just another kind of failure for a unit test

namespace VSL
{

class IVsContextualIntellisenseFilterProviderNotImpl :
	public IVsContextualIntellisenseFilterProvider
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IVsContextualIntellisenseFilterProviderNotImpl)

public:

	typedef IVsContextualIntellisenseFilterProvider Interface;

	STDMETHOD(GetFilter)(
		/*[in]*/ IVsHierarchy* /*pHierarchy*/,
		/*[out]*/ IVsContextualIntellisenseFilter** /*ppFilter*/)VSL_STDMETHOD_NOTIMPL
};

class IVsContextualIntellisenseFilterProviderMockImpl :
	public IVsContextualIntellisenseFilterProvider,
	public MockBase
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IVsContextualIntellisenseFilterProviderMockImpl)

public:

VSL_DEFINE_MOCK_CLASS_TYPDEFS(IVsContextualIntellisenseFilterProviderMockImpl)

	typedef IVsContextualIntellisenseFilterProvider Interface;
	struct GetFilterValidValues
	{
		/*[in]*/ IVsHierarchy* pHierarchy;
		/*[out]*/ IVsContextualIntellisenseFilter** ppFilter;
		HRESULT retValue;
	};

	STDMETHOD(GetFilter)(
		/*[in]*/ IVsHierarchy* pHierarchy,
		/*[out]*/ IVsContextualIntellisenseFilter** ppFilter)
	{
		VSL_DEFINE_MOCK_METHOD(GetFilter)

		VSL_CHECK_VALIDVALUE_INTERFACEPOINTER(pHierarchy);

		VSL_SET_VALIDVALUE_INTERFACE(ppFilter);

		VSL_RETURN_VALIDVALUES();
	}
};


} // namespace VSL

#pragma warning(pop)

#endif // IVSCONTEXTUALINTELLISENSEFILTERPROVIDER_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
