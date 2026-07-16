/**
 * Tipo que define os tipos de transações financeiras suportados.
 * 0 representa Receita e 1 representa Despesa.
 */
export type TipoTransacao = 0 | 1;

/**
 * Constante que mapeia os tipos de transação aos seus respectivos valores numéricos.
 */
export const TipoTransacao = {
  /**
   * Entrada de valores (Receita).
   */
  Receita: 0 as const,
  /**
   * Saída de valores (Despesa).
   */
  Despesa: 1 as const,
};

/**
 * Representa os dados cadastrais de uma pessoa.
 */
export interface Pessoa {
  /**
   * Identificador único autogerado.
   */
  id?: number;
  /**
   * Nome completo da pessoa.
   */
  nome: string;
  /**
   * Idade em anos.
   */
  idade: number;
}

/**
 * Representa os dados de uma transação financeira.
 */
export interface Transacao {
  /**
   * Identificador único autogerado.
   */
  id?: number;
  /**
   * Descrição detalhada da transação.
   */
  descricao: string;
  /**
   * Valor monetário da transação.
   */
  valor: number;
  /**
   * Tipo da transação (Receita ou Despesa).
   */
  tipo: TipoTransacao;
  /**
   * Identificador da pessoa proprietária da transação.
   */
  personId: number;
  /**
   * Pessoa proprietária.
   */
  pessoa?: Pessoa;
}

/**
 * Consolidação financeira individualizada de uma pessoa.
 */
export interface PessoaTotal {
  /**
   * Identificador único da pessoa.
   */
  id: number;
  /**
   * Nome da pessoa.
   */
  nome: string;
  /**
   * Idade da pessoa.
   */
  idade: number;
  /**
   * Soma de todas as receitas da pessoa.
   */
  totalReceitas: number;
  /**
   * Soma de todas as despesas da pessoa.
   */
  totalDespesas: number;
  /**
   * Saldo líquido individual (receitas - despesas).
   */
  saldo: number;
}

/**
 * Consolidação financeira geral para exibição no dashboard.
 */
export interface DashboardTotals {
  /**
   * Lista contendo os totais de cada pessoa.
   */
  pessoas: PessoaTotal[];
  /**
   * Soma das receitas de todas as pessoas.
   */
  totalGeralReceitas: number;
  /**
   * Soma das despesas de todas as pessoas.
   */
  totalGeralDespesas: number;
  /**
   * Saldo acumulado do sistema (receitas gerais - despesas gerais).
   */
  saldoGeral: number;
}
