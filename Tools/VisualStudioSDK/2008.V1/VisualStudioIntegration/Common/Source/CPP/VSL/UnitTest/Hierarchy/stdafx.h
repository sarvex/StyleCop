/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

#pragma once

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

#pragma warning(push)

// REVIEW - some or all of these /analyze warnings in the
// common headers should be fixed for RTM, re-enable the
// ones that are
#pragma warning(disable : 6011 6054 6309 6387 6535 6387 4505)

#define _ATL_NO_HOSTING

#include <atlbase.h>
#include <atlcom.h>
#include <atlstr.h>
#include <atlwin.h>

#include <commctrl.h>

#pragma warning(pop)

#include <VsShellInterfaces.h>

#define VSLASSERT _ASSERTE
#define VSLASSERTEX(exp, szMsg) _ASSERT_BASE(exp, szMsg)
#define VSLTRACE ATLTRACE

#include "VSLUnitTest.h"
#include "VSLMockSystemInterfaces.h"
#include "VSLMockVisualStudioInterfaces.h"
#include "VSLShortNameDefines.h"
#include <VSLMockIOleCommandTarget.h>

