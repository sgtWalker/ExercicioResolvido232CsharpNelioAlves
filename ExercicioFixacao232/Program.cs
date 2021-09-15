using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ExercicioFixacao232.Entities;

namespace ExercicioFixacao232
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string caminhoArquivo = SolicitarCarminhoArquivo();
                ValidarExtensao(caminhoArquivo);
                var produtos = ProcessarArquivo(caminhoArquivo);
                ImprimirPrecoMedio(produtos);
                ImprimirProdutoComPrecoInferiorAoMedio(produtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : "")}");
            }

        }

        private static string SolicitarCarminhoArquivo()
        {
            try
            {
                Console.Write("Entre com o caminho completo do arquivo: ");
                string caminhoArquivo = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(caminhoArquivo))
                    throw new Exception("Informe o caminho completo do arquivo!");

                return caminhoArquivo;
            }
            catch (Exception ex)
            {
                throw new Exception($"Método: SolicitarCaminhoArquivo, erro: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "")}");
            }
        }
        private static void ValidarExtensao(string caminhoArquivo)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(caminhoArquivo);
            if (directoryInfo.Extension != ".txt")
                throw new IOException("Informe apenas arquivos com a extensão .txt!");
        }

        private static List<Produto> ProcessarArquivo(string caminhoArquivo)
        {
            try
            {
                List<Produto> produtos = new List<Produto>();

                using (var sr = File.OpenText(caminhoArquivo))
                {
                    while (!sr.EndOfStream)
                    {
                        string linha = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(linha))
                        {
                            var quebraLinha = linha.Split(',');
                            produtos.Add(new Produto(quebraLinha[0], double.Parse(quebraLinha[1], CultureInfo.InvariantCulture)));
                        }
                    }
                }

                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Método: ProcessarArquivo, erro: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "")}");
            }
        }

        private static void ImprimirPrecoMedio(List<Produto> produtos)
        {
            var precoMedio = RetornarPrecoMedio(produtos);
            Console.WriteLine($"Preço médio: {precoMedio.ToString("F2", CultureInfo.InvariantCulture)}");
        }

        private static void ImprimirProdutoComPrecoInferiorAoMedio(List<Produto> produtos)
        {
            var precoMedio = RetornarPrecoMedio(produtos);
            var produtosSelecionados = produtos
                .Where(p => p.Preco < precoMedio)
                .OrderByDescending(p => p.Nome)
                .Select(p => p.Nome)
                .ToList();

            produtosSelecionados.ForEach(p => Console.WriteLine($"{p}"));
        }

        private static double RetornarPrecoMedio(List<Produto> produtos)
        {
            return produtos.Select(p => p.Preco).DefaultIfEmpty(0.0).Average();
        }
    }
}
