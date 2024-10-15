﻿using WebApiBia.Model;
using WebApiBia.ORM;

namespace WebApiBia.Repositorio
{
    public class Produto_Repositorio
    {
        private ProjetoBancoContext _context;

        public Produto_Repositorio(ProjetoBancoContext context)
        {
            _context = context;
        }

        public void Add(Produto produto, IFormFile NotaFiscal)
        {
            // Verifica se uma foto foi enviada
            byte[] NotaFiscalBytes = null;
            if (NotaFiscal != null && NotaFiscal.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    NotaFiscal.CopyTo(memoryStream);
                    NotaFiscalBytes = memoryStream.ToArray();
                }
            }

            // Cria uma nova entidade do tipo TbFuncionario a partir do objeto Funcionario recebido
            var tbProduto = new TbProduto()
            {
                Nome = produto.Nome,
                Preco = produto.Preco,
                Quantidade = produto.Quantidade,
                NotaFiscal= NotaFiscalBytes // Armazena a foto na entidade
            };

            // Adiciona a entidade ao contexto
            _context.TbProdutos.Add(tbProduto);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbProduto = _context.TbProdutos.FirstOrDefault(f => f.Id == id);

            // Verifica se a entidade foi encontrada
            if (tbProduto != null)
            {
                // Remove a entidade do contexto
                _context.TbProdutos.Remove(tbProduto);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Produto não encontrado.");
            }
        }

        public List<Produto> GetAll()
        {
            List<Produto> listFun = new List<Produto>();

            var listTb = _context.TbProdutos.ToList();

            foreach (var item in listTb)
            {
                var produto = new Produto
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Preco = item.Preco,
                    Quantidade = item.Quantidade,
                    NotaFiscal = item.NotaFiscal,
                    
                };

                listFun.Add(produto);
            }

            return listFun;
        }

        public Produto GetById(int id)
        {
            // Busca o funcionário pelo ID no banco de dados
            var item = _context.TbProdutos.FirstOrDefault(f => f.Id == id);

            // Verifica se o funcionário foi encontrado
            if (item == null)
            {
                return null; // Retorna null se não encontrar
            }

            // Mapeia o objeto encontrado para a classe Funcionario
            var produto = new Produto
            {
                
                    Id = item.Id,
                    Nome = item.Nome,
                    Preco = item.Preco,
                    Quantidade = item.Quantidade,
                    NotaFiscal = item.NotaFiscal, // Mantém o campo Foto como byte[]

            };
            return produto; // Retorna o funcionário encontrado
        }

        public void Update(Produto produto, IFormFile NotaFiscal)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbProduto = _context.TbProdutos.FirstOrDefault(f => f.Id == produto.Id);

            // Verifica se a entidade foi encontrada
            if (tbProduto != null)
            {
                // Atualiza os campos da entidade com os valores do objeto Funcionario recebido
                tbProduto.Nome = produto.Nome;
                tbProduto.Preco = produto.Preco;
                tbProduto.Quantidade = produto.Quantidade;
                tbProduto.NotaFiscal = produto.NotaFiscal;

                // Verifica se uma nova foto foi enviada
                if (NotaFiscal != null && NotaFiscal.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        NotaFiscal.CopyTo(memoryStream);
                        tbProduto.NotaFiscal = memoryStream.ToArray(); // Atualiza a foto na entidade
                    }
                }

                // Atualiza as informações no contexto
                _context.TbProdutos.Update(tbProduto);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Produto não encontrado.");
            }
        }
    }
}