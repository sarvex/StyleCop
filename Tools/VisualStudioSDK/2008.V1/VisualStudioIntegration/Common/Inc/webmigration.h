

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0499 */
/* at Tue Nov 06 20:05:30 2007
 */
/* Compiler settings for webmigration.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

/* verify that the <rpcsal.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCSAL_H_VERSION__
#define __REQUIRED_RPCSAL_H_VERSION__ 100
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __webmigration_h__
#define __webmigration_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IVsWebMigrationService_FWD_DEFINED__
#define __IVsWebMigrationService_FWD_DEFINED__
typedef interface IVsWebMigrationService IVsWebMigrationService;
#endif 	/* __IVsWebMigrationService_FWD_DEFINED__ */


#ifndef __IVsWebMigration_FWD_DEFINED__
#define __IVsWebMigration_FWD_DEFINED__
typedef interface IVsWebMigration IVsWebMigration;
#endif 	/* __IVsWebMigration_FWD_DEFINED__ */


#ifndef __IVsWebAppMigration_FWD_DEFINED__
#define __IVsWebAppMigration_FWD_DEFINED__
typedef interface IVsWebAppMigration IVsWebAppMigration;
#endif 	/* __IVsWebAppMigration_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "vsshell80.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_webmigration_0000_0000 */
/* [local] */ 

#define VSWEBMIGRATION_VER_MAJ    9
#define VSWEBMIGRATION_VER_MIN    0


extern RPC_IF_HANDLE __MIDL_itf_webmigration_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_webmigration_0000_0000_v0_0_s_ifspec;


#ifndef __VSWebMigration_LIBRARY_DEFINED__
#define __VSWebMigration_LIBRARY_DEFINED__

/* library VSWebMigration */
/* [version][helpstring][uuid] */ 


#define SID_SVsWebMigrationService IID_IVsWebMigrationService
DEFINE_GUID(CLSID_WebAppMigration, 0x26fa29e5, 0x8a81, 0x45e0, 0xae, 0xeb, 0xb, 0x1d, 0x6e, 0x5d, 0xd9, 0x77);

EXTERN_C const IID LIBID_VSWebMigration;

#ifndef __IVsWebMigrationService_INTERFACE_DEFINED__
#define __IVsWebMigrationService_INTERFACE_DEFINED__

/* interface IVsWebMigrationService */
/* [object][version][uuid] */ 


EXTERN_C const IID IID_IVsWebMigrationService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("DB0AD857-2F21-40c2-80F2-7CB9300F9DCA")
    IVsWebMigrationService : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE MigrateWeb( 
            /* [in] */ __RPC__in_opt IVsProject *pIVsProj,
            /* [in] */ __RPC__in LPCOLESTR pszLocation,
            /* [in] */ __RPC__in LPCOLESTR pszProjFile) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE IsWebProject( 
            /* [in] */ __RPC__in LPCOLESTR pszProjFile,
            /* [out] */ __RPC__out BOOL *pIsWeb) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetProjectSCCInfo( 
            /* [in] */ __RPC__in BSTR bstrProjectFIle,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccProjectName,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccAuxPath,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccLocalPath,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrProvider) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IVsWebMigrationServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IVsWebMigrationService * This,
            /* [in] */ __RPC__in REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IVsWebMigrationService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IVsWebMigrationService * This);
        
        HRESULT ( STDMETHODCALLTYPE *MigrateWeb )( 
            IVsWebMigrationService * This,
            /* [in] */ __RPC__in_opt IVsProject *pIVsProj,
            /* [in] */ __RPC__in LPCOLESTR pszLocation,
            /* [in] */ __RPC__in LPCOLESTR pszProjFile);
        
        HRESULT ( STDMETHODCALLTYPE *IsWebProject )( 
            IVsWebMigrationService * This,
            /* [in] */ __RPC__in LPCOLESTR pszProjFile,
            /* [out] */ __RPC__out BOOL *pIsWeb);
        
        HRESULT ( STDMETHODCALLTYPE *GetProjectSCCInfo )( 
            IVsWebMigrationService * This,
            /* [in] */ __RPC__in BSTR bstrProjectFIle,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccProjectName,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccAuxPath,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrSccLocalPath,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrProvider);
        
        END_INTERFACE
    } IVsWebMigrationServiceVtbl;

    interface IVsWebMigrationService
    {
        CONST_VTBL struct IVsWebMigrationServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IVsWebMigrationService_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IVsWebMigrationService_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IVsWebMigrationService_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IVsWebMigrationService_MigrateWeb(This,pIVsProj,pszLocation,pszProjFile)	\
    ( (This)->lpVtbl -> MigrateWeb(This,pIVsProj,pszLocation,pszProjFile) ) 

#define IVsWebMigrationService_IsWebProject(This,pszProjFile,pIsWeb)	\
    ( (This)->lpVtbl -> IsWebProject(This,pszProjFile,pIsWeb) ) 

#define IVsWebMigrationService_GetProjectSCCInfo(This,bstrProjectFIle,pbstrSccProjectName,pbstrSccAuxPath,pbstrSccLocalPath,pbstrProvider)	\
    ( (This)->lpVtbl -> GetProjectSCCInfo(This,bstrProjectFIle,pbstrSccProjectName,pbstrSccAuxPath,pbstrSccLocalPath,pbstrProvider) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IVsWebMigrationService_INTERFACE_DEFINED__ */


#ifndef __IVsWebMigration_INTERFACE_DEFINED__
#define __IVsWebMigration_INTERFACE_DEFINED__

/* interface IVsWebMigration */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IVsWebMigration;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1D1851BE-913D-40f4-AD7C-AD1F69A34E27")
    IVsWebMigration : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE LoadAssembly( 
            /* [in] */ __RPC__in BSTR bstrFilePath) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE GetBaseType( 
            /* [in] */ __RPC__in BSTR bstrClassName,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrBaseClass) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Unload( void) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IVsWebMigrationVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IVsWebMigration * This,
            /* [in] */ __RPC__in REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IVsWebMigration * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IVsWebMigration * This);
        
        HRESULT ( STDMETHODCALLTYPE *LoadAssembly )( 
            IVsWebMigration * This,
            /* [in] */ __RPC__in BSTR bstrFilePath);
        
        HRESULT ( STDMETHODCALLTYPE *GetBaseType )( 
            IVsWebMigration * This,
            /* [in] */ __RPC__in BSTR bstrClassName,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrBaseClass);
        
        HRESULT ( STDMETHODCALLTYPE *Unload )( 
            IVsWebMigration * This);
        
        END_INTERFACE
    } IVsWebMigrationVtbl;

    interface IVsWebMigration
    {
        CONST_VTBL struct IVsWebMigrationVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IVsWebMigration_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IVsWebMigration_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IVsWebMigration_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IVsWebMigration_LoadAssembly(This,bstrFilePath)	\
    ( (This)->lpVtbl -> LoadAssembly(This,bstrFilePath) ) 

#define IVsWebMigration_GetBaseType(This,bstrClassName,pbstrBaseClass)	\
    ( (This)->lpVtbl -> GetBaseType(This,bstrClassName,pbstrBaseClass) ) 

#define IVsWebMigration_Unload(This)	\
    ( (This)->lpVtbl -> Unload(This) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IVsWebMigration_INTERFACE_DEFINED__ */


#ifndef __IVsWebAppMigration_INTERFACE_DEFINED__
#define __IVsWebAppMigration_INTERFACE_DEFINED__

/* interface IVsWebAppMigration */
/* [unique][uuid][object] */ 


EXTERN_C const IID IID_IVsWebAppMigration;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("D1A24B44-033E-445f-B3AA-BE3F23C5617C")
    IVsWebAppMigration : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE CanConvertToWebApp( 
            /* [in] */ __RPC__in LPCOLESTR lpszProjectPath,
            /* [out] */ __RPC__out BOOL *pConverToWebApp,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrNewProjPath) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE ConvertProjFileToWebAppProj( 
            /* [in] */ __RPC__in LPCOLESTR lpszProjectPath,
            /* [out] */ __RPC__out GUID *pProjectFactoryGuid) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IVsWebAppMigrationVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IVsWebAppMigration * This,
            /* [in] */ __RPC__in REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IVsWebAppMigration * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IVsWebAppMigration * This);
        
        HRESULT ( STDMETHODCALLTYPE *CanConvertToWebApp )( 
            IVsWebAppMigration * This,
            /* [in] */ __RPC__in LPCOLESTR lpszProjectPath,
            /* [out] */ __RPC__out BOOL *pConverToWebApp,
            /* [out] */ __RPC__deref_out_opt BSTR *pbstrNewProjPath);
        
        HRESULT ( STDMETHODCALLTYPE *ConvertProjFileToWebAppProj )( 
            IVsWebAppMigration * This,
            /* [in] */ __RPC__in LPCOLESTR lpszProjectPath,
            /* [out] */ __RPC__out GUID *pProjectFactoryGuid);
        
        END_INTERFACE
    } IVsWebAppMigrationVtbl;

    interface IVsWebAppMigration
    {
        CONST_VTBL struct IVsWebAppMigrationVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IVsWebAppMigration_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IVsWebAppMigration_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IVsWebAppMigration_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IVsWebAppMigration_CanConvertToWebApp(This,lpszProjectPath,pConverToWebApp,pbstrNewProjPath)	\
    ( (This)->lpVtbl -> CanConvertToWebApp(This,lpszProjectPath,pConverToWebApp,pbstrNewProjPath) ) 

#define IVsWebAppMigration_ConvertProjFileToWebAppProj(This,lpszProjectPath,pProjectFactoryGuid)	\
    ( (This)->lpVtbl -> ConvertProjFileToWebAppProj(This,lpszProjectPath,pProjectFactoryGuid) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IVsWebAppMigration_INTERFACE_DEFINED__ */

#endif /* __VSWebMigration_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


