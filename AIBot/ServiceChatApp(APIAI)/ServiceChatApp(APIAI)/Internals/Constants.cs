namespace ServiceChatApp_APIAI_
{
    internal class Constants
    {
        //Application ID to connect it to LUIS

        public const string LUIS_APP_ID = "802d023d-fa45-4617-9c93-a737678d40e9";

        //Subscription key to call LUIS AI

        public const string LUIS_SUBSCRIPTION_ID = "94f553ac4df246ceb20cd904d7dc36f7";

        //Acces token for API.AI

        public const string APIAI_SUBSCRIPTION_ID = "05fc1f8d158849b8ab058d30aa249544";

        // Analytics KEY to authenticate the Text analytics API

        public const string TEXT_ANALYTICS_ID = "6b338c677a704354b740596015ad444a";

        public static string ServiceNowUserName = "admin";
        public static string ServiceNowPassword = "XyzoPw0NJ3qC";
        public static string ServiceNowUrl = "https://dev56432.service-now.com/api/now/table/";
        public static string ServiceNowJournalURL = "https://dev56432.service-now.com/api/now/table/sys_journal_field";
        public static object ServiceNowSubCategory = "Incident";
        public static object ServiceNowAssignmentGroup = "Service Desk";
        public static object ServiceNowIncidentImpact = "2";
        public static object ServiceNowIncidentPriority = "2";
        public static object ServiceNowCallerId = "SysAdminUser";
        public static object ServiceNowCatalogueName = "My bot application";
        public static object ServiceNowComments = "Incident created"; 
    }
}