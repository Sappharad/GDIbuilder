// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace GDIBuilderMac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton btnAddCdda { get; set; }

		[Outlet]
		AppKit.NSButton btnAdvanced { get; set; }

		[Outlet]
		AppKit.NSButton btnBrowseData { get; set; }

		[Outlet]
		AppKit.NSButton btnBrowseIpBin { get; set; }

		[Outlet]
		AppKit.NSButton btnBrowseOutput { get; set; }

		[Outlet]
		AppKit.NSButton btnCreate { get; set; }

		[Outlet]
		AppKit.NSButton btnDown { get; set; }

		[Outlet]
		AppKit.NSButton btnRemoveCdda { get; set; }

		[Outlet]
		AppKit.NSButton btnUp { get; set; }

		[Outlet]
		AppKit.NSButton chkRawMode { get; set; }

		[Outlet]
		AppKit.NSButton chkTruncateMode { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator pbProgress { get; set; }

		[Outlet]
		AppKit.NSTableView tblCdda { get; set; }

		[Outlet]
		AppKit.NSTextField txtApplicationId { get; set; }

		[Outlet]
		AppKit.NSTextField txtData { get; set; }

		[Outlet]
		AppKit.NSTextField txtDataPreparer { get; set; }

		[Outlet]
		AppKit.NSTextField txtIpBin { get; set; }

		[Outlet]
		AppKit.NSTextField txtOutput { get; set; }

		[Outlet]
		AppKit.NSTextField txtPublisherId { get; set; }

		[Outlet]
		AppKit.NSTextField txtResult { get; set; }

		[Outlet]
		AppKit.NSTextField txtSystemId { get; set; }

		[Outlet]
		AppKit.NSTextField txtVolSetId { get; set; }

		[Outlet]
		AppKit.NSTextField txtVolumeId { get; set; }

		[Outlet]
		AppKit.NSWindow winAdvanced { get; set; }

		[Outlet]
		AppKit.NSWindow winFinished { get; set; }

		[Action ("btnAddCdda_Clicked:")]
		partial void btnAddCdda_Clicked (Foundation.NSObject sender);

		[Action ("btnAdvanced_Clicked:")]
		partial void btnAdvanced_Clicked (Foundation.NSObject sender);

		[Action ("btnAdvancedCancel_Clicked:")]
		partial void btnAdvancedCancel_Clicked (Foundation.NSObject sender);

		[Action ("btnAdvancedOk_Clicked:")]
		partial void btnAdvancedOk_Clicked (Foundation.NSObject sender);

		[Action ("btnBrowseData_Clicked:")]
		partial void btnBrowseData_Clicked (Foundation.NSObject sender);

		[Action ("btnBrowseIpBin_Clicked:")]
		partial void btnBrowseIpBin_Clicked (Foundation.NSObject sender);

		[Action ("btnBrowseOutput_Clicked:")]
		partial void btnBrowseOutput_Clicked (Foundation.NSObject sender);

		[Action ("btnCreate_Clicked:")]
		partial void btnCreate_Clicked (Foundation.NSObject sender);

		[Action ("btnDown_Clicked:")]
		partial void btnDown_Clicked (Foundation.NSObject sender);

		[Action ("btnFinishedOk_Clicked:")]
		partial void btnFinishedOk_Clicked (Foundation.NSObject sender);

		[Action ("btnRemoveCdda_Clicked:")]
		partial void btnRemoveCdda_Clicked (Foundation.NSObject sender);

		[Action ("btnUp_Clicked:")]
		partial void btnUp_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAddCdda != null) {
				btnAddCdda.Dispose ();
				btnAddCdda = null;
			}

			if (btnAdvanced != null) {
				btnAdvanced.Dispose ();
				btnAdvanced = null;
			}

			if (btnBrowseData != null) {
				btnBrowseData.Dispose ();
				btnBrowseData = null;
			}

			if (btnBrowseIpBin != null) {
				btnBrowseIpBin.Dispose ();
				btnBrowseIpBin = null;
			}

			if (btnBrowseOutput != null) {
				btnBrowseOutput.Dispose ();
				btnBrowseOutput = null;
			}

			if (btnCreate != null) {
				btnCreate.Dispose ();
				btnCreate = null;
			}

			if (btnDown != null) {
				btnDown.Dispose ();
				btnDown = null;
			}

			if (btnRemoveCdda != null) {
				btnRemoveCdda.Dispose ();
				btnRemoveCdda = null;
			}

			if (btnUp != null) {
				btnUp.Dispose ();
				btnUp = null;
			}

			if (chkRawMode != null) {
				chkRawMode.Dispose ();
				chkRawMode = null;
			}

			if (chkTruncateMode != null) {
				chkTruncateMode.Dispose ();
				chkTruncateMode = null;
			}

			if (pbProgress != null) {
				pbProgress.Dispose ();
				pbProgress = null;
			}

			if (tblCdda != null) {
				tblCdda.Dispose ();
				tblCdda = null;
			}

			if (txtApplicationId != null) {
				txtApplicationId.Dispose ();
				txtApplicationId = null;
			}

			if (txtData != null) {
				txtData.Dispose ();
				txtData = null;
			}

			if (txtDataPreparer != null) {
				txtDataPreparer.Dispose ();
				txtDataPreparer = null;
			}

			if (txtIpBin != null) {
				txtIpBin.Dispose ();
				txtIpBin = null;
			}

			if (txtOutput != null) {
				txtOutput.Dispose ();
				txtOutput = null;
			}

			if (txtPublisherId != null) {
				txtPublisherId.Dispose ();
				txtPublisherId = null;
			}

			if (txtResult != null) {
				txtResult.Dispose ();
				txtResult = null;
			}

			if (txtSystemId != null) {
				txtSystemId.Dispose ();
				txtSystemId = null;
			}

			if (txtVolSetId != null) {
				txtVolSetId.Dispose ();
				txtVolSetId = null;
			}

			if (txtVolumeId != null) {
				txtVolumeId.Dispose ();
				txtVolumeId = null;
			}

			if (winAdvanced != null) {
				winAdvanced.Dispose ();
				winAdvanced = null;
			}

			if (winFinished != null) {
				winFinished.Dispose ();
				winFinished = null;
			}
		}
	}
}
