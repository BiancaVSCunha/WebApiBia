﻿    using WebApiBia.Model;
using WebApiBia.ORM;

namespace WebApiBia.Repositorio
{
    public class Cliente_Repositorio
    {
        private ProjetoBancoContext _context;

        public Cliente_Repositorio(ProjetoBancoContext context)
        {
            _context = context;
        }

        public void Add(Cliente cliente, IFormFile DocIdentificacao)
        {
            // Verifica se uma foto foi enviada
            byte[] DocIdentificacaoBytes = null;
            if (DocIdentificacao != null && DocIdentificacao.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    DocIdentificacao.CopyTo(memoryStream);
                    DocIdentificacaoBytes = memoryStream.ToArray();
                }
            }

            // Cria uma nova entidade do tipo TbFuncionario a partir do objeto Funcionario recebido
            var tbCliente = new TbCliente()
            {
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                DocIdentificacao = DocIdentificacaoBytes // Armazena a foto na entidade
            };

            // Adiciona a entidade ao contexto
            _context.TbClientes.Add(tbCliente);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbCliente = _context.TbClientes.FirstOrDefault(f => f.Id == id);

            // Verifica se a entidade foi encontrada
            if (tbCliente != null)
            {
                // Remove a entidade do contexto
                _context.TbClientes.Remove(tbCliente);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cliente não encontrado.");
            }
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> listFun = new List<Cliente>();

            var listTb = _context.TbClientes.ToList();

            foreach (var item in listTb)
            {
                var cliente = new Cliente
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Telefone = item.Telefone
                };

                listFun.Add(cliente);
            }

            return listFun;
        }

        public Cliente GetById(int id)
        {
            // Busca o funcionário pelo ID no banco de dados
            var item = _context.TbClientes.FirstOrDefault(f => f.Id == id);

            // Verifica se o funcionário foi encontrado
            if (item == null)
            {
                return null; // Retorna null se não encontrar
            }

            // Mapeia o objeto encontrado para a classe Funcionario
            var cliente = new Cliente
            {
                Id = item.Id,
                Nome = item.Nome,
                Telefone = item.Telefone,
                DocIdentificacao = item.DocIdentificacao // Mantém o campo Foto como byte[]
            };
            return cliente; // Retorna o funcionário encontrado
        }

        public void Update(Cliente cliente, IFormFile DocIdentificacao)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbCliente = _context.TbClientes.FirstOrDefault(f => f.Id == cliente.Id);

            // Verifica se a entidade foi encontrada
            if (tbCliente != null)
            {
                // Atualiza os campos da entidade com os valores do objeto Funcionario recebido
                tbCliente.Nome = cliente.Nome;
                tbCliente.Telefone = cliente.Telefone;

                // Verifica se uma nova foto foi enviada
                if (DocIdentificacao != null && DocIdentificacao.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        DocIdentificacao.CopyTo(memoryStream);
                        tbCliente.DocIdentificacao = memoryStream.ToArray(); // Atualiza a foto na entidade
                    }
                }

                // Atualiza as informações no contexto
                _context.TbClientes.Update(tbCliente);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cliente não encontrado.");
            }
        }
    }
}
