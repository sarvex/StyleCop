/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#ifndef IRELEASEMARSHALBUFFERS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
#define IRELEASEMARSHALBUFFERS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5

#if _MSC_VER > 1000
#pragma once
#endif

#include "ObjIdl.h"

#pragma warning(push)
#pragma warning(disable : 4510) // default constructor could not be generated
#pragma warning(disable : 4610) // can never be instantiated - user defined constructor required
#pragma warning(disable : 4512) // assignment operator could not be generated
#pragma warning(disable : 6011) // Dereferencing NULL pointer (a NULL derference is just another kind of failure for a unit test

namespace VSL
{

class IReleaseMarshalBuffersNotImpl :
	public IReleaseMarshalBuffers
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IReleaseMarshalBuffersNotImpl)

public:

	typedef IReleaseMarshalBuffers Interface;

	STDMETHOD(ReleaseMarshalBuffer)(
		/*[in]*/ RPCOLEMESSAGE* /*pMsg*/,
		/*[in]*/ DWORD /*dwFlags*/,
		/*[in,unique]*/ IUnknown* /*pChnl*/)VSL_STDMETHOD_NOTIMPL
};

class IReleaseMarshalBuffersMockImpl :
	public IReleaseMarshalBuffers,
	public MockBase
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IReleaseMarshalBuffersMockImpl)

public:

VSL_DEFINE_MOCK_CLASS_TYPDEFS(IReleaseMarshalBuffersMockImpl)

	typedef IReleaseMarshalBuffers Interface;
	struct ReleaseMarshalBufferValidValues
	{
		/*[in]*/ RPCOLEMESSAGE* pMsg;
		/*[in]*/ DWORD dwFlags;
		/*[in,unique]*/ IUnknown* pChnl;
		HRESULT retValue;
	};

	STDMETHOD(ReleaseMarshalBuffer)(
		/*[in]*/ RPCOLEMESSAGE* pMsg,
		/*[in]*/ DWORD dwFlags,
		/*[in,unique]*/ IUnknown* pChnl)
	{
		VSL_DEFINE_MOCK_METHOD(ReleaseMarshalBuffer)

		VSL_CHECK_VALIDVALUE_POINTER(pMsg);

		VSL_CHECK_VALIDVALUE(dwFlags);

		VSL_CHECK_VALIDVALUE_INTERFACEPOINTER(pChnl);

		VSL_RETURN_VALIDVALUES();
	}
};


} // namespace VSL

#pragma warning(pop)

#endif // IRELEASEMARSHALBUFFERS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
