namespace ConsoleVeiling
{
    internal class Bod
    {
        private decimal _bedrag;
        private Koper _koper;

        public decimal Bedrag {
            get { return _bedrag; }
        }
        public Koper Koper { get; set; }

        public Bod(Koper koper, decimal bedrag)
        {
            _bedrag = bedrag;
            _koper = koper;
        }
    }
}
