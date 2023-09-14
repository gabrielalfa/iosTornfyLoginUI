package crc64b24e1a3c7ad90e58;


public class ActionSheetAppCompatDialogFragment
	extends crc64b24e1a3c7ad90e58.AbstractAppCompatDialogFragment_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCancel:(Landroid/content/DialogInterface;)V:GetOnCancel_Landroid_content_DialogInterface_Handler\n" +
			"n_dismiss:()V:GetDismissHandler\n" +
			"";
		mono.android.Runtime.register ("Acr.UserDialogs.Extended.Platforms.Android.Fragments.ActionSheetAppCompatDialogFragment, Acr.UserDialogs.Extended", ActionSheetAppCompatDialogFragment.class, __md_methods);
	}


	public ActionSheetAppCompatDialogFragment ()
	{
		super ();
		if (getClass () == ActionSheetAppCompatDialogFragment.class) {
			mono.android.TypeManager.Activate ("Acr.UserDialogs.Extended.Platforms.Android.Fragments.ActionSheetAppCompatDialogFragment, Acr.UserDialogs.Extended", "", this, new java.lang.Object[] {  });
		}
	}


	public void onCancel (android.content.DialogInterface p0)
	{
		n_onCancel (p0);
	}

	private native void n_onCancel (android.content.DialogInterface p0);


	public void dismiss ()
	{
		n_dismiss ();
	}

	private native void n_dismiss ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
