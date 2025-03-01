
using ResortManagement.Web.ViewModels;

namespace Internship_Portal.Utility
{
    public class SD
    {
        public const string Role_Student = "Student";
        public const string Role_Admin = "Admin";
        public const string Role_Mentor = "Mentor";
        public const string Role_TNP = "TNP";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusInternshipSubmited = "Internship Submited For Approval";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        public static RadialBarChartDto GetRadialChartDataModel(int totalCount, double currentMonthCount, double prevMonthCount)
        {
            RadialBarChartDto radialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            radialBarChartVM.HasRatioIncreased = currentMonthCount > prevMonthCount;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }

    }
}
