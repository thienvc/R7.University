//
//  ListControlExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace R7.University.ControlExtensions
{
    public static class ListControlExtensions
    {
        public static bool SelectByText (this ListControl listControl, string text, StringComparison comparison)
        {
            if (!string.IsNullOrWhiteSpace (text)) {
                foreach (ListItem item in listControl.Items) {
                    if (string.Compare (item.Text, text, comparison) == 0) {
                        item.Selected = true;
                        return true;
                    }
                }
            }

            return false;
        }

        // TODO: Move to base library
        public static IEnumerable<ListItem> AsEnumerable (this ListItemCollection itemCollection)
        {
            var enumerator = itemCollection.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                yield return (ListItem) enumerator.Current;
            }
        }
    }
}
