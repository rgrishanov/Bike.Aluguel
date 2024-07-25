﻿using Bike.Dto.Equipamento;

namespace BikeApi.Aplicacao.AluguelServico
{
	public interface IIntegracaoEquipamentoServico
	{
		public void AlterarStatusBicicleta(int idBicicleta, string novoStatus);
		public void AlterarStatusTranca(int idTranca, string novoStatus);
		public bool DestrancarTranca(int idTranca);
		public BicicletaNaTrancaDto ObterBicicletaNaTranca(int idTranca);
	}
}
