namespace AzureEmbodiedAISamples
{
    public struct SampleViseme
    {
        public ulong AudioOffset;
        public uint VisemeId;

        public SampleViseme(ulong audioOffset, uint visemeId)
        {
            this.AudioOffset = audioOffset;
            this.VisemeId = visemeId;
        }
    }
}