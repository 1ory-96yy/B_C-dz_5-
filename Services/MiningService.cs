using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1.Services
{
    public class MiningService
    {
        private readonly HashingService _hashingService;

        public MiningService()
        {
            _hashingService = new HashingService();
        }

        public void MineBlock(Block block, int difficulty)
        {

            var target = new string('0', difficulty);
            while (true)
            {
                if (block.nonce % 100000 == 0)
                {
                    Console.Write($".");
                }
                block.hash = _hashingService.ComputeHash(block);
                if (block.hash.StartsWith(target))
                {
                    break;
                }
                block.nonce++;
            }
        }
    }
}
