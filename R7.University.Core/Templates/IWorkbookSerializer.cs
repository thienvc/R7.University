using System.Text;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    public interface IWorkbookSerializer
    {
        StringBuilder Serialize (IWorkbook book, StringBuilder builder);
    }
}
