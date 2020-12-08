namespace CA201208D
{
    public struct Focista
    {
        public readonly byte MezSzam;
        public readonly string Nev;
        public readonly string Nemzetiseg;
        public readonly string Poszt;
        public readonly ushort SzulEv;

        public Focista (byte mezSzam, string nev, string nemzetiseg, string poszt, ushort szulEv)
        {
            MezSzam = mezSzam;
            Nev = nev;
            Nemzetiseg = nemzetiseg;
            Poszt = poszt;
            SzulEv = szulEv;
        }
    }
}