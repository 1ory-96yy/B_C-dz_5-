namespace ConsoleApp1.Models
{
    public class transaction
    {
        public string id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public double amount { get; set; }
        public DateTime timestamp { get; set; }

        public byte[] SenderPublicKey { get; set; }
        public byte[]? Signature { get; set; }

        public transaction(string from, string to, double amount)
        {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.timestamp = DateTime.Now;
            this.id = Guid.NewGuid().ToString();
        }

        public byte[] GetDataForSigning()
        {
            var dataString = $"{from}{to}{amount}{timestamp:yyyyMMddHHmmss}";
            return System.Text.Encoding.UTF8.GetBytes(dataString);
        }

        public string ToRowString()
        {
            string signatureEX = Signature != null ? Convert.ToHexString(Signature) : "null";
            return $"{id}\t{from}\t{to}\t{amount}\t{timestamp}\t{signatureEX}";
        }
        public override string ToString()
        {
            return $"Transaction from {from} to {to} of amount {amount} at {timestamp}";
        }


    }
}
