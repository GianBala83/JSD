// Program.cs
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Loja;

public class Program
{
    public static void Main(string[] args)
    {
        string jsonFilePath = "lista_produtos.json";
        List<Produtos> prds = CarregarDeJson(jsonFilePath);

        bool v = true;
        while (v)
        {
            Console.WriteLine("Escolha sua opção: ");
            Console.WriteLine("1 - Adicionar Produto");
            Console.WriteLine("2 - Remover Produto");
            Console.WriteLine("3 - Listar Produtos");
            Console.WriteLine("4 - Listar Produtos por Preço");
            Console.WriteLine("5 - Encerrar Programa");

            string input = Console.ReadLine();
            int op;
            bool resp = int.TryParse(input, out op);
            while (!resp || (op > 5 || op < 1))
            {
                Console.WriteLine("Resposta Inválido");
                input = Console.ReadLine();
                resp = int.TryParse(input, out op);
            }

            switch (op)
            {
                case 1:
                    Console.Clear();
                    Console.Write("Nome do Produto:  ");
                    string input_name = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(input_name) ||  TaNaLista(prds,input_name))
                    {
                        Console.Clear();
                        if (string.IsNullOrWhiteSpace(input_name))
                        {
                            Console.WriteLine("Resposta Inválida\n");
                        }
                        else
                        {
                            Console.WriteLine("Resposta Inválida, nome ja em uso\n");
                        }    
                        
                        Console.Write("Nome do Produto:  ");
                        input_name = Console.ReadLine();
                    }
                    Console.Write("Preço do Produto:  ");
                    string input_value = Console.ReadLine();
                    input_value = input_value.Replace(',', '.');
                    bool resp2 = float.TryParse(input_value, NumberStyles.Float, CultureInfo.InvariantCulture, out float value);
                    while (!resp2)
                    {
                        Console.Clear();
                        Console.WriteLine($"Nome do produto:  {input_name}");
                        Console.WriteLine("\nPreço Inválido\n");
                        Console.Write("Preço do Produto:  ");
                        input_value = Console.ReadLine();
                        input_value = input_value.Replace(',', '.');
                        resp2 = float.TryParse(input_value, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
                    }
                    AddProduto(prds, input_name, value);
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para voltar ao menu");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 2:
                    Console.Clear();
                    ListarProdutos(prds);
                    Console.Write("\nID do Produto:  ");
                    bool resp3 = int.TryParse(Console.ReadLine(),out int input_id);
                    while (!resp3 || input_id < 0 || input_id > prds.Count - 1)
                    {
                        Console.Clear();
                        ListarProdutos(prds);
                        Console.WriteLine("\nID Inválido, digite outro ID\n");
                        Console.Write("ID do Produto:  ");
                        resp3 = int.TryParse(Console.ReadLine(),out input_id);
                    }
                    
                    RemoveProduto(prds, input_id);
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para voltar ao menu");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 3:
                    Console.Clear();
                    ListarProdutos(prds);
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para voltar ao menu");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 4:
                    Console.Clear();
                    ListarProdutosOrd(prds);
                    Console.WriteLine();
                    Console.WriteLine("Pressione Enter para voltar ao menu");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 5:
                    SalvarEmJson(prds, jsonFilePath);
                    v = false;
                    break;
            }
        }
    }

    public static void AddProduto(List<Produtos> product, string name, float value)
    {
        Produtos prd = new Produtos(name, value);
        if (product.Count == 0)
        {
            prd.id = 0;
        }
        else
        {
            prd.id = product[product.Count-1].id + 1;
        }
        product.Add(prd);
        Console.WriteLine("Produto adicionado com sucesso");
    }

    public static void RemoveProduto(List<Produtos> product, int ID)
    {
        //Produtos p = product.Find(p => p.nome == name);
        if (ID < 0 || ID > product.Count - 1 )
        {
            Console.WriteLine("Produto não encontrado");
        }
        else
        {
            product.RemoveAt(ID);
            
            foreach (var i in product)
            {
                if (i.id > ID)
                {
                    i.id -= 1;
                }
            }
            Console.WriteLine("Produto removido com sucesso");
        }
    }

    public static void ListarProdutos(List<Produtos> product)
    {
        foreach (var i in product)
        {
            if (i.id < 10)
            {
                Console.WriteLine($"00{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
            else if (i.id < 100)
            {
                Console.WriteLine($"0{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
            else
            {
                Console.WriteLine($"{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
        }
    }

    public static void ListarProdutosOrd(List<Produtos> product)
    {
        List<Produtos> produc = new List<Produtos>(product);
        produc.Sort();
        foreach (var i in produc)
        {
            if (i.id < 10)
            {
                Console.WriteLine($"00{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
            else if (i.id < 100)
            {
                Console.WriteLine($"0{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
            else
            {
                Console.WriteLine($"{i.id} - {i.nome} - R${i.valor.ToString("F2")}");
            }
        }
        Console.WriteLine();
    }

    public static void SalvarEmJson(List<Produtos> product, string filePath)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(product);
            File.WriteAllText(filePath, jsonString);
            Console.WriteLine("Produtos salvos em arquivo JSON com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar o arquivo JSON: {ex.Message}");
        }
    }

    public static List<Produtos> CarregarDeJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Produtos>>(jsonString);
        }
        return new List<Produtos>();
    }

    public static bool TaNaLista(List<Produtos> prod, string name)
    {
        foreach (var i in prod)
        {
            if (i.nome == name)
            {
                return true;
            }
        }

        return false;
    }
}
