﻿using Bike.Dominio.Ciclista.Validacao;
using Bike.Dominio.Validacao;
using Bike.Dto.Ciclista;

namespace BikeApi.Dominio.Ciclista
{
	public class Ciclista
	{
		public int Id { get; private set; }
		public string Nome { get; private set; }
		public DateTime Nascimento { get; private set; }
		public string Cpf { get; private set; }
		public Passaporte? Passaporte { get; private set; }
		public string Nacionalidade { get; private set; }
		public string Email { get; private set; }
		public string UrlFotoDocumento { get; private set; }
		public string Senha { get; private set; }
        public string Status { get; private set; }  // 'AGUARDANDO_CONFIRMACAO', 'ATIVO', 'INATIVO',
        public DateTime AtivoDesde { get; set; }

        public Ciclista(CiclistaDto dto)
		{
			Validador.Validar(dto, new CiclistaValidacao());

			this.PreencherCamposBasicos(dto);

			Status = "AGUARDANDO_CONFIRMACAO";

			if (this.Nacionalidade!.ToUpperInvariant().Equals("ESTRANGEIRO"))
				Passaporte = new Passaporte(dto.Passaporte!);
		}

		public void SetarIdInicial(int id)
		{
			if (this.Id == 0)
				this.Id = id;
			else
				throw new ArgumentException("Não é possível alterar o Id do Ciclista.");
		}

		public void AtivarCadastro()
		{
			switch (this.Status)
			{
				case "AGUARDANDO_CONFIRMACAO":
					this.Status = "ATIVO";
					this.AtivoDesde = DateTime.Now;
					break;
				case "INATIVO":
					throw new ArgumentException("Este ciclista está Inativo.");
				case "ATIVO":
					throw new ArgumentException("Este ciclista já está Ativo.");
				default:
					throw new ArgumentException("Status indefinido, erro impossível.");
			}
		}

		public void Alterar(CiclistaDto dto)
		{
			Validador.Validar(dto, new CiclistaValidacao());

			if (this.Nacionalidade.ToUpperInvariant().Equals("ESTRANGEIRO"))
				Passaporte = new Passaporte(dto.Passaporte!);

			this.PreencherCamposBasicos(dto);		
		}

		private void PreencherCamposBasicos(CiclistaDto dto)
		{
			this.Nome = dto.Nome!;
			this.Nascimento = dto.Nascimento!.Value;
			this.Cpf = dto.Cpf!;
			this.Nacionalidade = dto.Nacionalidade!;
			this.Email = dto.Email!;
			this.UrlFotoDocumento = dto.UrlFotoDocumento!;
			this.Senha = dto.Senha!;
		}
	}
}
