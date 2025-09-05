using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Identity.Interfaces;
public interface ISecureRandom
{
    byte[] GetBytes(int length);
    string GetBase64(int length);
}