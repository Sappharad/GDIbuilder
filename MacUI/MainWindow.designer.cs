// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace MacUI
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnAddCdda { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnAdvanced { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnBrowseData { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnBrowseIpBin { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnBrowseOutput { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnCreate { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnDown { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnRemoveCdda { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnUp { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton chkRawMode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator pbProgress { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView tblCdda { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtApplicationId { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtData { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtDataPreparer { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtIpBin { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtOutput { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtPublisherId { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtResult { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtSystemId { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtVolSetId { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtVolumeId { get; set; }

		[Outlet]
		MonoMac.AppKit.NSWindow winAdvanced { get; set; }

		[Outlet]
		MonoMac.AppKit.NSWindow winFinished { get; set; }

		[Action ("btnAdd_Clicked:")]
		partial void btnAdd_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnAdvanced_Clicked:")]
		partial void btnAdvanced_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnAdvCancel_Clicked:")]
		partial void btnAdvCancel_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnAdvOK_Clicked:")]
		partial void btnAdvOK_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnBrowseData_Clicked:")]
		partial void btnBrowseData_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnBrowseIpBin_Clicked:")]
		partial void btnBrowseIpBin_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnBrowseOutput_Clicked:")]
		partial void btnBrowseOutput_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnCreate_Clicked:")]
		partial void btnCreate_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnDown_Clicked:")]
		partial void btnDown_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnFinishedOK_Clicked:")]
		partial void btnFinishedOK_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnRemove_Clicked:")]
		partial void btnRemove_Clicked (MonoMac.Foundation.NSObject sender);

		[Action ("btnUp_Clicked:")]
		partial void btnUp_Clicked (MonoMac.Foundation.NSObject sender);
		
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

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
