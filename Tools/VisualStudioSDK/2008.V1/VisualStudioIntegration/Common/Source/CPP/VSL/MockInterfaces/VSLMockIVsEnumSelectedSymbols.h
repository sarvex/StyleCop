/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#ifndef IVSENUMSELECTEDSYMBOLS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
#define IVSENUMSELECTEDSYMBOLS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5

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

class IVsEnumSelectedSymbolsNotImpl :
	public IVsEnumSelectedSymbols
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IVsEnumSelectedSymbolsNotImpl)

public:

	typedef IVsEnumSelectedSymbols Interface;

	STDMETHOD(Next)(
		/*[in]*/ ULONG /*celt*/,
		/*[out,size_is(celt),length_is(*pceltFetched)]*/ IVsSelectedSymbol** /*rgpIVsSelectedSymbol*/,
		/*[out]*/ ULONG* /*pceltFetched*/)VSL_STDMETHOD_NOTIMPL

	STDMETHOD(Skip)(
		/*[in]*/ ULONG /*celt*/)VSL_STDMETHOD_NOTIMPL

	STDMETHOD(Reset)()VSL_STDMETHOD_NOTIMPL

	STDMETHOD(Clone)(
		/*[out]*/ IVsEnumSelectedSymbols** /*ppIVsEnumSelectedSymbols*/)VSL_STDMETHOD_NOTIMPL
};

class IVsEnumSelectedSymbolsMockImpl :
	public IVsEnumSelectedSymbols,
	public MockBase
{

VSL_DECLARE_NONINSTANTIABLE_BASE_CLASS(IVsEnumSelectedSymbolsMockImpl)

public:

VSL_DEFINE_MOCK_CLASS_TYPDEFS(IVsEnumSelectedSymbolsMockImpl)

	typedef IVsEnumSelectedSymbols Interface;
	struct NextValidValues
	{
		/*[in]*/ ULONG celt;
		/*[out,size_is(celt),length_is(*pceltFetched)]*/ IVsSelectedSymbol** rgpIVsSelectedSymbol;
		/*[out]*/ ULONG* pceltFetched;
		HRESULT retValue;
	};

	STDMETHOD(Next)(
		/*[in]*/ ULONG celt,
		/*[out,size_is(celt),length_is(*pceltFetched)]*/ IVsSelectedSymbol** rgpIVsSelectedSymbol,
		/*[out]*/ ULONG* pceltFetched)
	{
		VSL_DEFINE_MOCK_METHOD(Next)

		VSL_CHECK_VALIDVALUE(celt);

		VSL_SET_VALIDVALUE_INTERFACEARRAY(rgpIVsSelectedSymbol, celt, *(validValues.pceltFetched));

		VSL_SET_VALIDVALUE(pceltFetched);

		VSL_RETURN_VALIDVALUES();
	}
	struct SkipValidValues
	{
		/*[in]*/ ULONG celt;
		HRESULT retValue;
	};

	STDMETHOD(Skip)(
		/*[in]*/ ULONG celt)
	{
		VSL_DEFINE_MOCK_METHOD(Skip)

		VSL_CHECK_VALIDVALUE(celt);

		VSL_RETURN_VALIDVALUES();
	}
	struct ResetValidValues
	{
		HRESULT retValue;
	};

	STDMETHOD(Reset)()
	{
		VSL_DEFINE_MOCK_METHOD_NOARGS(Reset)

		VSL_RETURN_VALIDVALUES();
	}
	struct CloneValidValues
	{
		/*[out]*/ IVsEnumSelectedSymbols** ppIVsEnumSelectedSymbols;
		HRESULT retValue;
	};

	STDMETHOD(Clone)(
		/*[out]*/ IVsEnumSelectedSymbols** ppIVsEnumSelectedSymbols)
	{
		VSL_DEFINE_MOCK_METHOD(Clone)

		VSL_SET_VALIDVALUE_INTERFACE(ppIVsEnumSelectedSymbols);

		VSL_RETURN_VALIDVALUES();
	}
};


} // namespace VSL

#pragma warning(pop)

#endif // IVSENUMSELECTEDSYMBOLS_H_10C49CA1_2F46_11D3_A504_00C04F5E0BA5
