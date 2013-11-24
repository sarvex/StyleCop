// VsDebugGuids.h
//

#ifdef SHOW_INCLUDES
#pragma message("Includes " __FILE__)
#endif

//#ifndef __GUIDS_H_
//#define __GUIDS_H_

#ifdef SHOW_INCLUDES
#pragma message("+++INCLUDING " __FILE__)
#endif


#ifndef _CTC_GUIDS_

#include "objext.h"        // for ILocalRegistry
#include "oleipc.h"        // for ComponentUIManager


// {A27B4E24-A735-4D1D-B8E7-9716E1E3D8E0}
DEFINE_GUID(guidTextEditorFontCategory,
            0xA27B4E24, 0xA735, 0x4D1D, 0xB8, 0xE7, 0x97, 0x16, 0xE1, 0xE3, 0xD8, 0xE0);

// {C9DD4A58-47FB-11d2-83E7-00C04F9902C1}
DEFINE_GUID(guidVSDebugGroup,
            0xC9DD4A58, 0x47FB, 0x11D2, 0x83, 0xE7, 0x00, 0xC0, 0x4F, 0x99, 0x02, 0xC1);

// {C9DD4A59-47FB-11d2-83E7-00C04F9902C1}
DEFINE_GUID(guidVSDebugCommand,
            0xC9DD4A59, 0x47FB, 0x11D2, 0x83, 0xE7, 0x00, 0xC0, 0x4F, 0x99, 0x02, 0xC1);

// {FA9EB535-C624-13D0-AE1F-00A0190FF4C3}
DEFINE_GUID(guidDbgOptGeneralPage,
            0xfa9eb535, 0xc624, 0x13d0, 0xae, 0x1f, 0x00, 0xa0, 0x19, 0x0f, 0xf4, 0xc3);

// {7A8A4060-D909-4485-9860-748BC8713A74}
DEFINE_GUID(guidDbgOptFindSourcePage,
            0x7a8a4060, 0xd909, 0x4485, 0x98, 0x60, 0x74, 0x8b, 0xc8, 0x71, 0x3a, 0x74);

// {C15095AA-49C0-40AC-AE78-611318DD9925}
DEFINE_GUID(guidDbgOptFindSymbolPage,
            0xC15095AA, 0x49C0, 0x40AC, 0xAE, 0x78, 0x61, 0x13, 0x18, 0xDD, 0x99, 0x25);

// {6C3ECAA6-3EFB-4b0d-9660-2A3BA5B8440E}
DEFINE_GUID(guidDbgOptENCPage,
            0x6c3ecaa6, 0x3efb, 0x4b0d, 0x96, 0x60, 0x2a, 0x3b, 0xa5, 0xb8, 0x44, 0xe);

// {B9EFCAF2-9EAE-4022-9E39-FA947666ADD9}
DEFINE_GUID(guidDbgOptJITPage,
            0xb9efcaf2, 0x9eae, 0x4022, 0x9e, 0x39, 0xfa, 0x94, 0x76, 0x66, 0xad, 0xd9);

// {1F5E080F-CBD2-459C-8267-39fd83032166}
DEFINE_GUID(guidDbgOptSymbolPage,
            0x1f5e080f, 0xcbd2, 0x459c, 0x82, 0x67, 0x39, 0xfd, 0x83, 0x03, 0x21, 0x66);

// {FC076020-078A-11D1-A7DF-00A0C9110051}
DEFINE_GUID(guidDebugOutputPane,
            0xfc076020, 0x078a, 0x11d1, 0xa7, 0xdf, 0x00, 0xa0, 0xc9, 0x11, 0x00, 0x51);

// {C16FB7C4-9F84-11D2-8405-00C04F9902C1}
DEFINE_GUID(guidDisasmLangSvc,
            0xc16fb7c4, 0x9f84, 0x11d2, 0x84, 0x05, 0x00, 0xc0, 0x4f, 0x99, 0x02, 0xc1);

// {3BFC1046-049F-11d3-B87F-00C04F79E479}
DEFINE_GUID(guidMemoryView,
            0x3bfc1046, 0x49f, 0x11d3, 0xb8, 0x7f, 0x0, 0xc0, 0x4f, 0x79, 0xe4, 0x79);

// {DF38847E-CC19-11d2-8ADA-00C04F79E479}
DEFINE_GUID(guidMemoryLangSvc,
            0xdf38847e, 0xcc19, 0x11d2, 0x8a, 0xda, 0x0, 0xc0, 0x4f, 0x79, 0xe4, 0x79);

// {13F6A341-59C0-11d3-994C-00C04F68FDAF}
DEFINE_GUID(guidRegisterLangSvc,
            0x13f6a341, 0x59c0, 0x11d3, 0x99, 0x4c, 0x0, 0xc0, 0x4f, 0x68, 0xfd, 0xaf);

// {75058B12-F5A9-4b1c-9161-9B3754D7488F}
DEFINE_GUID(guidENCStaleLangSvc,
            0x75058b12, 0xf5a9, 0x4b1c, 0x91, 0x61, 0x9b, 0x37, 0x54, 0xd7, 0x48, 0x8f);


// {44B05627-95C2-4CE8-BDCD-4AA722785093}
DEFINE_GUID(guidDebuggerMarkerService,
            0x44b05627, 0x95c2, 0x4ce8, 0xbd, 0xcd, 0x4a, 0xa7, 0x22, 0x78, 0x50, 0x93);

// UNDONE: this should be defined by the environment in vsshell.idl
// {A2FE74E1-B743-11d0-AE1A-00A0C90FFFC3}
DEFINE_GUID(guidExternalFilesProject,
            0xa2fe74e1, 0xb743, 0x11d0, 0xae, 0x1a, 0x00, 0xa0, 0xc9, 0x0f, 0xff, 0xc3);

// {201BFBC6-D20B-11d2-910F-00C04F9902C1}
// this CmdUIContext is defined when the debugger is started for Just-In-Time debugging
DEFINE_GUID(guidJitDebug,
            0x201bfbc6, 0xd20b, 0x11d2, 0x91, 0x0f, 0x00, 0xc0, 0x4f, 0x99, 0x02, 0xc1);

// {E5776E42-0966-11d3-B87F-00C04F79E479}
// This is a private interface used by the memory view for communicating with a Language service.
DEFINE_GUID(IID_IMemoryViewLangServiceInterop,
            0xe5776e42, 0x966, 0x11d3, 0xb8, 0x7f, 0x0, 0xc0, 0x4f, 0x79, 0xe4, 0x79);

// {8C7DDC02-C7B5-4532-AB98-9AEC7C9E02FA}
DEFINE_GUID(guidENCOptionRelink,
            0x8c7ddc02, 0xc7b5, 0x4532, 0xab, 0x98, 0x9a, 0xec, 0x7c, 0x9e, 0x2, 0xfa);

// {C46344BE-C093-4672-AAFC-80012715798C}
DEFINE_GUID(guidENCOptionPrecompile,
            0xc46344be, 0xc093, 0x4672, 0xaa, 0xfc, 0x80, 0x1, 0x27, 0x15, 0x79, 0x8c);

// {EE71B5E6-1FE6-4f14-8D73-0981BC4CF5BA}
DEFINE_GUID(guidENCOptionNativeApplyOnContinue,
            0xee71b5e6, 0x1fe6, 0x4f14, 0x8d, 0x73, 0x9, 0x81, 0xbc, 0x4c, 0xf5, 0xba);

// {ABA46DCE-94D3-469f-A785-D7B529C5B1B7}
DEFINE_GUID(guidENCOptionNativeAllowRemote, 
            0xaba46dce, 0x94d3, 0x469f, 0xa7, 0x85, 0xd7, 0xb5, 0x29, 0xc5, 0xb1, 0xb7);

// {ce2eced5-c21c-464c-9b45-15e10e9f9ef9}
DEFINE_GUID(guidFontColorMemory,
            0xce2eced5, 0xc21c, 0x464c, 0x9b, 0x45, 0x15, 0xe1, 0x0e, 0x9f, 0x9e, 0xf9);

// {40660f54-80fa-4375-89a3-8d06aa954eba}
DEFINE_GUID(guidFontColorRegisters,
            0x40660f54, 0x80fa, 0x4375, 0x89, 0xa3, 0x8d, 0x06, 0xaa, 0x95, 0x4e, 0xba);

// {3B70A4AE-BB91-4abe-A05C-C4DE07B9763E}
DEFINE_GUID(guidDebuggerFontColorSvc,
            0x3b70a4ae, 0xbb91, 0x4abe, 0xa0, 0x5c, 0xc4, 0xde, 0x7, 0xb9, 0x76, 0x3e);

// {358463D0-D084-400f-997E-A34FC570BC72}
DEFINE_GUID(guidWatchFontColor,
            0x358463d0, 0xd084, 0x400f, 0x99, 0x7e, 0xa3, 0x4f, 0xc5, 0x70, 0xbc, 0x72);

// {A7EE6BEE-D0AA-4b2f-AD9D-748276A725F6}
DEFINE_GUID(guidAutosFontColor,
            0xa7ee6bee, 0xd0aa, 0x4b2f, 0xad, 0x9d, 0x74, 0x82, 0x76, 0xa7, 0x25, 0xf6);

// {8259ACED-490A-41b3-A0FB-64C842CCDC80}
DEFINE_GUID(guidLocalsFontColor,
            0x8259aced, 0x490a, 0x41b3, 0xa0, 0xfb, 0x64, 0xc8, 0x42, 0xcc, 0xdc, 0x80);

// {E02A3CCD-2D8E-4628-97D7-1C0921DFA2F3}
DEFINE_GUID(guidParallelWatchFontColor,
            0xe02a3ccd, 0x2d8e, 0x4628, 0x97, 0xd7, 0x1c, 0x9, 0x21, 0xdf, 0xa2, 0xf3);

// {FD2219AF-EBF8-4116-A801-3B503C48DFF0}
DEFINE_GUID(guidCallStackFontColor,
            0xfd2219af, 0xebf8, 0x4116, 0xa8, 0x1, 0x3b, 0x50, 0x3c, 0x48, 0xdf, 0xf0);

// {BB8FE807-A186-404a-81FA-D20B908CA93B}
DEFINE_GUID(guidThreadsFontColor,
            0xbb8fe807, 0xa186, 0x404a, 0x81, 0xfa, 0xd2, 0xb, 0x90, 0x8c, 0xa9, 0x3b);

// {F7B7B222-E186-48df-A5EE-174E8129891B}
DEFINE_GUID(guidDataTipsFontColor,
            0xf7b7b222, 0xe186, 0x48df, 0xa5, 0xee, 0x17, 0x4e, 0x81, 0x29, 0x89, 0x1b);

// {B20C0001-0836-4535-A5E8-96E595B1F094}
DEFINE_GUID(guidDebugLocationFontColor, 
            0xb20c0001, 0x836, 0x4535, 0xa5, 0xe8, 0x96, 0xe5, 0x95, 0xb1, 0xf0, 0x94);


// {35B25E75-AB53-4c5d-80EA-6682EBB2BBBD}
DEFINE_GUID(guidVarWndsFontColor,
            0x35b25e75, 0xab53, 0x4c5d, 0x80, 0xea, 0x66, 0x82, 0xeb, 0xb2, 0xbb, 0xbd);

// {8DAFF493-5F7C-4e19-81BF-D5E63C1545D3}
DEFINE_GUID(guidProjectLaunchSettings,
			0x8daff493, 0x5f7c, 0x4e19, 0x81, 0xbf, 0xd5, 0xe6, 0x3c, 0x15, 0x45, 0xd3);

// {60AFC91C-3AD5-4D33-8C00-D8EF5DEDDCD1}
DEFINE_GUID(guidITraceDebuggerService,
			0x60afc91c, 0x3ad5, 0x4d33, 0x8c, 0x00, 0xd8, 0xef, 0x5d, 0xed, 0xdc, 0xd1);

#else  // _CTC_GUIDS

#define guidVSDebugPackage        { 0xC9DD4A57, 0x47FB, 0x11D2, { 0x83, 0xE7, 0x00, 0xC0, 0x4F, 0x99, 0x02, 0xC1 } }
#define guidVSDebugGroup          { 0xC9DD4A58, 0x47FB, 0x11D2, { 0x83, 0xE7, 0x00, 0xC0, 0x4F, 0x99, 0x02, 0xC1 } }
#define guidVSDebugCommand        { 0xC9DD4A59, 0x47FB, 0x11D2, { 0x83, 0xE7, 0x00, 0xC0, 0x4F, 0x99, 0x02, 0xC1 } }

#define guidDbgOptGeneralPage     { 0xfa9eb535, 0xc624, 0x13d0, { 0xae, 0x1f, 0x00, 0xa0, 0x19, 0x0f, 0xf4, 0xc3 } }
#define guidDbgOptFindSourcePage  { 0x7a8a4060, 0xd909, 0x4485, { 0x98, 0x60, 0x74, 0x8b, 0xc8, 0x71, 0x3a, 0x74 } }
#define guidDbgOptFindSymbolPage  { 0xc15095aa, 0x49c0, 0x40ac, { 0xae, 0x78, 0x61, 0x13, 0x18, 0xdd, 0x99, 0x25 } }
#define guidDbgOptENCPage         { 0x6c3ecaa6, 0x3efb, 0x4b0d, { 0x96, 0x60, 0x2a, 0x3b, 0xa5, 0xb8, 0x44, 0x0e } }
#define guidDbgOptJITPage         { 0xb9efcaf2, 0x9eae, 0x4022, { 0x9e, 0x39, 0xfa, 0x94, 0x76, 0x66, 0xad, 0xd9 } }

#define guidDebugOutputPane       { 0xfc076020, 0x078a, 0x11d1, { 0xa7, 0xdf, 0x00, 0xa0, 0xc9, 0x11, 0x00, 0x51 } }
#define guidDisasmLangSvc         { 0xc16fb7c4, 0x9f84, 0x11d2, { 0x84, 0x05, 0x00, 0xc0, 0x4f, 0x99, 0x02, 0xc1 } }
#define guidMemoryLangSvc         { 0xdf38847e, 0xcc19, 0x11d2, { 0x8a, 0xda, 0x00, 0xc0, 0x4f, 0x79, 0xe4, 0x79 } }

#define guidFontColorMemory       { 0xce2eced5, 0xc21c, 0x464c, { 0x9b, 0x45, 0x15, 0xe1, 0x0e, 0x9f, 0x9e, 0xf9 } }
#define guidFontColorRegisters    { 0x40660f54, 0x80fa, 0x4375, { 0x89, 0xa3, 0x8d, 0x06, 0xaa, 0x95, 0x4e, 0xba } }

#define guidDebuggerFontColorSvc  { 0x3b70a4ae, 0xbb91, 0x4abe, { 0xa0, 0x5c, 0xc4, 0xde, 0x7, 0xb9, 0x76, 0x3e } }
#define guidWatchFontColor    { 0x358463d0, 0xd084, 0x400f, { 0x99, 0x7e, 0xa3, 0x4f, 0xc5, 0x70, 0xbc, 0x72 } }
#define guidAutosFontColor    { 0xa7ee6bee, 0xd0aa, 0x4b2f, { 0xad, 0x9d, 0x74, 0x82, 0x76, 0xa7, 0x25, 0xf6 } }
#define guidLocalsFontColor    { 0x8259aced, 0x490a, 0x41b3, { 0xa0, 0xfb, 0x64, 0xc8, 0x42, 0xcc, 0xdc, 0x80 } }
#define guidParallelWatchFontColor    { 0xe02a3ccd, 0x2d8e, 0x4628, { 0x97, 0xd7, 0x1c, 0x9, 0x21, 0xdf, 0xa2, 0xf3 } }
#define guidCallStackFontColor   { 0xfd2219af, 0xebf8, 0x4116, { 0xa8, 0x1, 0x3b, 0x50, 0x3c, 0x48, 0xdf, 0xf0  } }
#define guidThreadsFontColor   { 0xbb8fe807, 0xa186, 0x404a, { 0x81, 0xfa, 0xd2, 0xb, 0x90, 0x8c, 0xa9, 0x3b  } }
#define guidDataTipsFontColor   { 0xf7b7b222, 0xe186, 0x48df, { 0xa5, 0xee, 0x17, 0x4e, 0x81, 0x29, 0x89, 0x1b } }
#define guidVarWndsFontColor   { 0x35b25e75, 0xab53, 0x4c5d, { 0x80, 0xea, 0x66, 0x82, 0xeb, 0xb2, 0xbb, 0xbd } };

#endif // _CTC_GUIDS_


//#endif // __GUIDS_H_
