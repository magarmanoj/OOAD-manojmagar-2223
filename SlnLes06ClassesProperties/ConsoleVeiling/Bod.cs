namespace ConsoleVeiling
{
    internal class Bod
    {
        public decimal Bedrag { get; set; }
        public Koper Koper { get; set; }

        public Bod(Koper koper, decimal bedrag)
        {
            Bedrag = bedrag;
            Koper = koper;
        }
    }
}
