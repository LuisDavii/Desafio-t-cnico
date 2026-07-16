import axios from 'axios';
import type { Pessoa, Transacao, DashboardTotals } from '../types';

const API_BASE_URL = 'http://localhost:5175/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Obtém a lista simples de todas as pessoas.
 * @returns Promessa com a lista de pessoas.
 */
export const getPessoas = async (): Promise<Pessoa[]> => {
  const response = await api.get<Pessoa[]>('/pessoas');
  return response.data;
};

/**
 * Cadastra uma nova pessoa.
 * @param pessoa Dados da pessoa a ser cadastrada.
 * @returns Promessa com a pessoa cadastrada.
 */
export const createPessoa = async (pessoa: Pessoa): Promise<Pessoa> => {
  const response = await api.post<Pessoa>('/pessoas', pessoa);
  return response.data;
};

/**
 * Exclui uma pessoa pelo seu identificador único.
 * @param id Identificador da pessoa.
 * @returns Promessa que resolve quando a exclusão é concluída.
 */
export const deletePessoa = async (id: number): Promise<void> => {
  await api.delete(`/pessoas/${id}`);
};

/**
 * Obtém o demonstrativo consolidado de totais individuais e gerais para o dashboard.
 * @returns Promessa com os totais do dashboard.
 */
export const getDashboardTotals = async (): Promise<DashboardTotals> => {
  const response = await api.get<DashboardTotals>('/pessoas/totais');
  return response.data;
};

/**
 * Obtém a lista de todas as transações cadastradas.
 * @returns Promessa com a lista de transações.
 */
export const getTransacoes = async (): Promise<Transacao[]> => {
  const response = await api.get<Transacao[]>('/transacoes');
  return response.data;
};

/**
 * Cadastra uma nova transação financeira.
 * @param transacao Dados da transação a ser registrada.
 * @returns Promessa com a transação criada.
 */
export const createTransacao = async (transacao: Transacao): Promise<Transacao> => {
  const response = await api.post<Transacao>('/transacoes', transacao);
  return response.data;
};

export default api;
