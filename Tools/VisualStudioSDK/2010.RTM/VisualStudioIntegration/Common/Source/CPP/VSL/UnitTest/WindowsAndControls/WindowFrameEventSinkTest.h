/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

This code is a part of the Visual Studio Library.

***************************************************************************/

class WindowFrameEventsSinkTest :
	public VSL::UnitTestBase
{
public:
	WindowFrameEventsSinkTest(_In_opt_ const char* const szTestName);

private:
	void CheckSubscribeUnsubscribe();
	void CheckEvents();
};
