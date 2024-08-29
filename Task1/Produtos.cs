using System;
using System.Text.Json.Serialization;
namespace Loja
{
    public class Produtos : IComparable<Produtos>
    {
        public int id { get; set; }
        public string nome { get; set; }
        public float valor { get; set; }
        

        public Produtos(string name, float value)
        {
            nome = name;
            valor = value;
        }
        public Produtos()
        {
            
        }
        public int CompareTo(Produtos outroProduto)
        {
            return valor.CompareTo(outroProduto.valor);
        }
        
    }
    
}

