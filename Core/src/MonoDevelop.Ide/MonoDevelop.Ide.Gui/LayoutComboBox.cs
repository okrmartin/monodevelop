//
// LayoutComboBox.cs
//
// Author:
//   David Makovský <yakeen@sannyas-on.net>
//
// Copyright (C) 2006 David Makovský
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using MonoDevelop.Ide;
using Gtk;

namespace MonoDevelop.Ide.Gui
{
	internal class LayoutComboBox : Alignment
	{
		ComboBox combo;
		bool changingHere = false;
	
		public LayoutComboBox ()
			: base (0.5f, 0.5f, 1.0f, 0f)
		{
			LeftPadding = 3;
			RightPadding = 3;
			combo = Gtk.ComboBox.NewText ();
			combo.Changed += new EventHandler (OnComboChanged);
			Add (combo);
			ShowAll ();
			
			IdeApp.Workbench.LayoutChanged += (EventHandler) Services.DispatchService.GuiDispatch (new EventHandler (OnConfigurationsChanged));
		}
		
		void OnConfigurationsChanged (object sender, EventArgs e)
		{
			((ListStore)combo.Model).Clear ();
			int active = 0;
			for (int i = 0; i < IdeApp.Workbench.Layouts.Length; i++)
			{
				combo.AppendText (IdeApp.Workbench.Layouts[i]);
				if (IdeApp.Workbench.Layouts[i] == IdeApp.Workbench.CurrentLayout)
					active = i;
			}
			changingHere = true;
			combo.Active = active;
			combo.ShowAll ();
		}
		
		void OnComboChanged (object sender, EventArgs e)
		{
			if (! changingHere)
			{
				TreeIter iter;
				if (combo.GetActiveIter (out iter))
					IdeApp.Workbench.CurrentLayout = (string)combo.Model.GetValue (iter, 0);
			}
			changingHere = false;
		}
	}
}
