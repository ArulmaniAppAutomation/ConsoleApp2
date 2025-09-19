using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Utils.BaseUtils.UtilInterface
{
    public interface InterfaceGrid
    {
        #region Grid Operations
        Task SetSearchBoxValueAsync(string value);
        Task DeleteProfileAsync(string value);
        Task ClickGridCellContextMenuAsync(string name);
        Task ClickGridCellContextMenuDeleteButtonAsync();
        Task ClickConfirmDeleteButtonAsync();
        Task VerifyProfileCreatedSuccessfullyOrNotAsync(string name);
        Task VerifyProfileDeletedSuccessfullyOrNotAsync(string name);
        #endregion

        #region Basics
        Task SetBasicsProfileNameAsync(string name);
        Task SetBasicsDescriptionAsync(string description);
        #endregion
    }
}
