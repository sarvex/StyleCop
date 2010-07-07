/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#ifndef IADDREXCLUSIONCONTROL_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
#define IADDREXCLUSIONCONTROL_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5

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

class IAddrExclusionControlNotImpl :
	public IAddrExclusionControl
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IAddrExclusionControlNotImpl)

public:

	typedef IAddrExclusionControl Interface;

	STDMETHOD(GetCurrentAddrExclusionList)(
		/*[in]*/ REFIID /*riid*/,
		/*[out,iid_is(riid)]*/ void** /*ppEnumerator*/)VSL_STDMETHOD_NOTIMPL

	STDMETHOD(UpdateAddrExclusionList)(
		/*[in]*/ IUnknown* /*pEnumerator*/)VSL_STDMETHOD_NOTIMPL
};

class IAddrExclusionControlMockImpl :
	public IAddrExclusionControl,
	public MockBase
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IAddrExclusionControlMockImpl)

public:

VSL_DEFINE_MOCK_CLASS_TYPDEFS(IAddrExclusionControlMockImpl)

	typedef IAddrExclusionControl Interface;
	struct GetCurrentAddrExclusionListValidValues
	{
		/*[in]*/ REFIID riid;
		/*[out,iid_is(riid)]*/ void** ppEnumerator;
		HRESULT retValue;
	};

	STDMETHOD(GetCurrentAddrExclusionList)(
		/*[in]*/ REFIID riid,
		/*[out,iid_is(riid)]*/ void** ppEnumerator)
	{
		VSL_DEFINE_MOCK_METHOD(GetCurrentAddrExclusionList)

		VSL_CHECK_VALIDVALUE(riid);

		VSL_SET_VALIDVALUE(ppEnumerator);

		VSL_RETURN_VALIDVALUES();
	}
	struct UpdateAddrExclusionListValidValues
	{
		/*[in]*/ IUnknown* pEnumerator;
		HRESULT retValue;
	};

	STDMETHOD(UpdateAddrExclusionList)(
		/*[in]*/ IUnknown* pEnumerator)
	{
		VSL_DEFINE_MOCK_METHOD(UpdateAddrExclusionList)

		VSL_CHECK_VALIDVALUE_INTERFACEPOINTER(pEnumerator);

		VSL_RETURN_VALIDVALUES();
	}
};


} // namespace VSL

#pragma warning(pop)

#endif // IADDREXCLUSIONCONTROL_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
