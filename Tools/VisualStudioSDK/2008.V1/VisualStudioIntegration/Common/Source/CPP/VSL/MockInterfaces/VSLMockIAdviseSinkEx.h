/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#ifndef IADVISESINKEX_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
#define IADVISESINKEX_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5

#if _MSC_VER > 1000
#pragma once
#endif

#include "OCIdl.h"

#pragma warning(push)
#pragma warning(disable : 4510) // default constructor could not be generated
#pragma warning(disable : 4610) // can never be instantiated - user defined constructor required
#pragma warning(disable : 4512) // assignment operator could not be generated
#pragma warning(disable : 6011) // Dereferencing NULL pointer (a NULL derference is just another kind of failure for a unit test

namespace VSL
{

class IAdviseSinkExNotImpl :
	public IAdviseSinkEx
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IAdviseSinkExNotImpl)

public:

	typedef IAdviseSinkEx Interface;

	virtual void STDMETHODCALLTYPE OnViewStatusChange(
		/*[in]*/ DWORD /*dwViewStatus*/){ return ; }

	virtual void STDMETHODCALLTYPE OnDataChange(
		/*[in,unique]*/ FORMATETC* /*pFormatetc*/,
		/*[in,unique]*/ STGMEDIUM* /*pStgmed*/){ return ; }

	virtual void STDMETHODCALLTYPE OnViewChange(
		/*[in]*/ DWORD /*dwAspect*/,
		/*[in]*/ LONG /*lindex*/){ return ; }

	virtual void STDMETHODCALLTYPE OnRename(
		/*[in]*/ IMoniker* /*pmk*/){ return ; }

	virtual void STDMETHODCALLTYPE OnSave(){ return ; }

	virtual void STDMETHODCALLTYPE OnClose(){ return ; }
};

class IAdviseSinkExMockImpl :
	public IAdviseSinkEx,
	public MockBase
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IAdviseSinkExMockImpl)

public:

VSL_DEFINE_MOCK_CLASS_TYPDEFS(IAdviseSinkExMockImpl)

	typedef IAdviseSinkEx Interface;
	struct OnViewStatusChangeValidValues
	{
		/*[in]*/ DWORD dwViewStatus;
	};

	virtual void _stdcall OnViewStatusChange(
		/*[in]*/ DWORD dwViewStatus)
	{
		VSL_DEFINE_MOCK_METHOD(OnViewStatusChange)

		VSL_CHECK_VALIDVALUE(dwViewStatus);

	}
	struct OnDataChangeValidValues
	{
		/*[in,unique]*/ FORMATETC* pFormatetc;
		/*[in,unique]*/ STGMEDIUM* pStgmed;
	};

	virtual void _stdcall OnDataChange(
		/*[in,unique]*/ FORMATETC* pFormatetc,
		/*[in,unique]*/ STGMEDIUM* pStgmed)
	{
		VSL_DEFINE_MOCK_METHOD(OnDataChange)

		VSL_CHECK_VALIDVALUE_POINTER(pFormatetc);

		VSL_CHECK_VALIDVALUE_POINTER(pStgmed);

	}
	struct OnViewChangeValidValues
	{
		/*[in]*/ DWORD dwAspect;
		/*[in]*/ LONG lindex;
	};

	virtual void _stdcall OnViewChange(
		/*[in]*/ DWORD dwAspect,
		/*[in]*/ LONG lindex)
	{
		VSL_DEFINE_MOCK_METHOD(OnViewChange)

		VSL_CHECK_VALIDVALUE(dwAspect);

		VSL_CHECK_VALIDVALUE(lindex);

	}
	struct OnRenameValidValues
	{
		/*[in]*/ IMoniker* pmk;
	};

	virtual void _stdcall OnRename(
		/*[in]*/ IMoniker* pmk)
	{
		VSL_DEFINE_MOCK_METHOD(OnRename)

		VSL_CHECK_VALIDVALUE_INTERFACEPOINTER(pmk);

	}

	virtual void _stdcall OnSave()
	{
		VSL_DEFINE_MOCK_METHOD_NOARGS_NORETURN(OnSave)

	}

	virtual void _stdcall OnClose()
	{
		VSL_DEFINE_MOCK_METHOD_NOARGS_NORETURN(OnClose)

	}
};


} // namespace VSL

#pragma warning(pop)

#endif // IADVISESINKEX_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
