namespace PlaywrightNUnitFramework.Locators
{
    public static class allLocators
    {
       
        public static string EmailInput => "#userEmail";
        public static string PasswordInput => "#userPassword";
        public static string LoginButton => "button[type='submit']";
        public static string LeaveManagement => "p.mt-4:text('Leave Management')";
        public static string RequestsButton => "button.leave-tab:has-text('Requests')";
        public static string LeftPinnedRows => ".ag-pinned-left-cols-container .ag-row";
        public static string CenterColsViewport => ".ag-center-cols-viewport";
        public static string CenterColsContainer => ".ag-center-cols-container";
        public static string EmployeeIdCell => "[col-id='employeeId']";
        public static string ApproveButton(string rowIndex) => $".ag-center-cols-container [row-index='{rowIndex}'] button.approve-btn";
        public static string ApplyLeaveButton => "button:text('Apply Leave')";
        public static string FromDateInput => "#fromDate";
        public static string ToDateInput => "#toDate";
        public static string SubjectInput => "input[name='subject']";
        public static string ReasonTextarea => "textarea[name='reason']";
        public static string LeaveTypeRadio => "input[name='requestType'][value='leave']";
        public static string LeadSelect => "select[name='lead']";
        public static string SubmitButton => "button[type='submit']";
        public static string LopModalOkButton => "div.modal-content >> button:text('Ok')";
        public static string ReimbursementLocator => "li:has-text(\"Reimbursement\")";
        public static string ApplyExtraWorkButtonLocator => "button.sc-iGgWBj.kDWecC:has-text(\"Apply Extra Work\")";
        public static string ExtraworkDateInputLocator => "input[name=\"date\"]";
        public static string HoursInputLocator => "input[type=\"text\"][name=\"hours\"]";
        public static string LeadSelectLocator => "select[name=\"lead\"]";
        public static string ExtraworkSubmitButtonLocator => "button:has-text(\"Submit\")";
        public static string ExtraWorkReimbursementTab => "text=Reimbursement";
        public static string ExtraWorkRequestsButton => "text=Requests";
        public static string ExtraWorkLeaveRows => ".ag-center-cols-container .ag-row";
        public static string ExtraWorkEmployeeIdCell => "[col-id='employeeId']";


    }
}










