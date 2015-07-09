
namespace MPViewer
{
    internal class ProgressInfo
    {
        private int m_percentageComplete;
        private string m_status;

        // Properties
        public int PercentageComplete
        {
            get
            {
                return this.m_percentageComplete;
            }
        }

        public string Status
        {
            get
            {
                return this.m_status;
            }
        }

        public ProgressInfo(string status, int percentageComplete)
        {
            this.m_status = status;
            this.m_percentageComplete = percentageComplete;
        }
    }
}
