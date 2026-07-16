import { useState, useEffect } from 'react';
import { 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Users, 
  PlusCircle, 
  Trash2, 
  LayoutDashboard, 
  Wallet, 
  AlertTriangle, 
  X,
  CheckCircle2
} from 'lucide-react';
import './App.css';
import { TipoTransacao } from './types';
import type { Pessoa, Transacao, DashboardTotals } from './types';
import { 
  getPessoas, 
  createPessoa, 
  deletePessoa, 
  getDashboardTotals, 
  getTransacoes, 
  createTransacao 
} from './services/api';

/**
 * Componente principal da aplicação que gerencia o estado e renderiza as visualizações de Dashboard, Pessoas e Transações.
 */
function App() {
  const [activeTab, setActiveTab] = useState<'dashboard' | 'pessoas' | 'transacoes'>('dashboard');
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [totals, setTotals] = useState<DashboardTotals | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [notification, setNotification] = useState<{ type: 'success' | 'danger'; message: string } | null>(null);

  const [pessoaForm, setPessoaForm] = useState<{ nome: string; idade: string }>({
    nome: '',
    idade: '',
  });

  const [transacaoForm, setTransacaoForm] = useState<{
    descricao: string;
    valor: string;
    tipo: TipoTransacao;
    personId: string;
  }>({
    descricao: '',
    valor: '',
    tipo: TipoTransacao.Despesa,
    personId: '',
  });

  /**
   * Efeito para carregar as informações do back-end ao inicializar a página.
   */
  useEffect(() => {
    loadAllData();
  }, []);

  /**
   * Carrega os dados de pessoas, transações e totais financeiros de forma paralela a partir da API.
   */
  const loadAllData = async () => {
    setLoading(true);
    try {
      const [pessoasData, transacoesData, totalsData] = await Promise.all([
        getPessoas(),
        getTransacoes(),
        getDashboardTotals()
      ]);
      setPessoas(pessoasData);
      setTransacoes(transacoesData);
      setTotals(totalsData);
    } catch {
      showNotification('danger', 'Erro ao carregar dados do servidor. Certifique-se de que a API está rodando.');
    } finally {
      setLoading(false);
    }
  };

  /**
   * Exibe uma notificação temporária na tela.
   * @param type Tipo da notificação (sucesso ou perigo).
   * @param message Mensagem a ser exibida.
   */
  const showNotification = (type: 'success' | 'danger', message: string) => {
    setNotification({ type, message });
    setTimeout(() => {
      setNotification(null);
    }, 6000);
  };

  /**
   * Formata valores decimais como moeda Real (BRL).
   * @param value O valor decimal numérico.
   * @returns O valor formatado em reais.
   */
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
  };

  /**
   * Trata o envio do formulário de criação de pessoa física.
   * @param e Evento de submissão do formulário.
   */
  const handleCreatePessoa = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!pessoaForm.nome.trim() || !pessoaForm.idade) {
      showNotification('danger', 'Preencha todos os campos do formulário de pessoa.');
      return;
    }

    const idadeNum = parseInt(pessoaForm.idade, 10);
    if (isNaN(idadeNum) || idadeNum < 0 || idadeNum > 150) {
      showNotification('danger', 'Por favor, informe uma idade válida (entre 0 e 150).');
      return;
    }

    try {
      await createPessoa({
        nome: pessoaForm.nome,
        idade: idadeNum,
      });
      setPessoaForm({ nome: '', idade: '' });
      showNotification('success', 'Pessoa cadastrada com sucesso!');
      await loadAllData();
    } catch {
      showNotification('danger', 'Falha ao cadastrar pessoa.');
    }
  };

  /**
   * Trata a solicitação de exclusão de uma pessoa física.
   * @param id Identificador único da pessoa a ser removida.
   */
  const handleDeletePessoa = async (id: number) => {
    const confirmDelete = window.confirm('Tem certeza de que deseja excluir esta pessoa? Todas as suas transações vinculadas serão apagadas em cascata.');
    if (!confirmDelete) return;

    try {
      await deletePessoa(id);
      showNotification('success', 'Pessoa e transações excluídas com sucesso.');
      await loadAllData();
    } catch {
      showNotification('danger', 'Falha ao excluir pessoa.');
    }
  };

  /**
   * Trata o envio do formulário de lançamento de transação financeira.
   * @param e Evento de submissão do formulário.
   */
  const handleCreateTransacao = async (e: React.FormEvent) => {
    e.preventDefault();
    const { descricao, valor, tipo, personId } = transacaoForm;

    if (!descricao.trim() || !valor || !personId) {
      showNotification('danger', 'Preencha todos os campos da transação.');
      return;
    }

    const valorNum = parseFloat(valor);
    if (isNaN(valorNum) || valorNum <= 0) {
      showNotification('danger', 'O valor da transação deve ser maior que zero.');
      return;
    }

    const personIdNum = parseInt(personId, 10);

    try {
      await createTransacao({
        descricao,
        valor: valorNum,
        tipo,
        personId: personIdNum,
      });
      setTransacaoForm({
        descricao: '',
        valor: '',
        tipo: TipoTransacao.Despesa,
        personId: '',
      });
      showNotification('success', 'Transação registrada com sucesso!');
      await loadAllData();
    } catch (err: any) {
      if (err.response?.data?.message) {
        showNotification('danger', err.response.data.message);
      } else {
        showNotification('danger', 'Falha ao registrar transação.');
      }
    }
  };

  return (
    <div className="container">
      <header className="header">
        <div className="logo-container">
          <Wallet className="logo-icon" size={32} />
          <span className="logo-title">Gastos Residenciais</span>
        </div>
        <nav className="nav-tabs">
          <button 
            className={`nav-tab ${activeTab === 'dashboard' ? 'active' : ''}`}
            onClick={() => setActiveTab('dashboard')}
          >
            <LayoutDashboard size={18} />
            Dashboard
          </button>
          <button 
            className={`nav-tab ${activeTab === 'pessoas' ? 'active' : ''}`}
            onClick={() => setActiveTab('pessoas')}
          >
            <Users size={18} />
            Pessoas
          </button>
          <button 
            className={`nav-tab ${activeTab === 'transacoes' ? 'active' : ''}`}
            onClick={() => setActiveTab('transacoes')}
          >
            <DollarSign size={18} />
            Transações
          </button>
        </nav>
      </header>

      {notification && (
        <div className={`notification ${notification.type}`}>
          {notification.type === 'success' ? (
            <CheckCircle2 className="notification-icon" size={20} />
          ) : (
            <AlertTriangle className="notification-icon" size={20} />
          )}
          <div className="notification-message">{notification.message}</div>
          <button className="notification-close" onClick={() => setNotification(null)}>
            <X size={16} />
          </button>
        </div>
      )}

      {loading ? (
        <div className="empty-state">
          <div className="logo-title">Carregando informações...</div>
        </div>
      ) : (
        <>
          {activeTab === 'dashboard' && (
            <div>
              <div className="dashboard-grid">
                <div className="card receitas">
                  <div className="card-header">
                    <span>Total Receitas</span>
                    <TrendingUp className="card-icon" size={20} />
                  </div>
                  <div className="card-value">
                    {formatCurrency(totals?.totalGeralReceitas ?? 0)}
                  </div>
                  <div className="card-subtext">Soma de todas as receitas domésticas</div>
                </div>

                <div className="card despesas">
                  <div className="card-header">
                    <span>Total Despesas</span>
                    <TrendingDown className="card-icon" size={20} />
                  </div>
                  <div className="card-value">
                    {formatCurrency(totals?.totalGeralDespesas ?? 0)}
                  </div>
                  <div className="card-subtext">Soma de todos os débitos registrados</div>
                </div>

                <div className={`card saldo ${(totals?.saldoGeral ?? 0) >= 0 ? 'positivo' : 'negativo'}`}>
                  <div className="card-header">
                    <span>Saldo Geral Acumulado</span>
                    <DollarSign className="card-icon" size={20} />
                  </div>
                  <div className="card-value">
                    {formatCurrency(totals?.saldoGeral ?? 0)}
                  </div>
                  <div className="card-subtext">Receita total líquida doméstica</div>
                </div>
              </div>

              <div className="ui-panel">
                <h3 className="section-title">
                  <Users size={20} />
                  Relatório Financeiro por Pessoa
                </h3>
                {totals && totals.pessoas.length > 0 ? (
                  <div className="table-responsive">
                    <table className="modern-table">
                      <thead>
                        <tr>
                          <th>Nome</th>
                          <th>Idade</th>
                          <th>Receitas</th>
                          <th>Despesas</th>
                          <th>Saldo</th>
                        </tr>
                      </thead>
                      <tbody>
                        {totals.pessoas.map((p) => (
                          <tr key={p.id}>
                            <td>{p.nome}</td>
                            <td>{p.idade} {p.idade === 1 ? 'ano' : 'anos'} {p.idade < 18 && <span className="badge despesa" style={{ marginLeft: 8 }}>Menor</span>}</td>
                            <td className="text-success">{formatCurrency(p.totalReceitas)}</td>
                            <td className="text-danger">{formatCurrency(p.totalDespesas)}</td>
                            <td className={p.saldo >= 0 ? 'text-success' : 'text-danger'}>
                              {formatCurrency(p.saldo)}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <div className="empty-state">
                    <Users className="empty-state-icon" size={48} />
                    <div className="empty-state-title">Nenhuma pessoa registrada</div>
                    <div className="empty-state-desc">Cadastre pessoas na aba Pessoas para iniciar o cálculo de totais.</div>
                  </div>
                )}
              </div>
            </div>
          )}

          {activeTab === 'pessoas' && (
            <div className="grid-content">
              <div className="ui-panel">
                <h3 className="section-title">
                  <Users size={20} />
                  Pessoas Cadastradas
                </h3>
                {pessoas.length > 0 ? (
                  <div className="table-responsive">
                    <table className="modern-table">
                      <thead>
                        <tr>
                          <th>ID</th>
                          <th>Nome</th>
                          <th>Idade</th>
                          <th style={{ width: '80px', textAlign: 'center' }}>Ações</th>
                        </tr>
                      </thead>
                      <tbody>
                        {pessoas.map((p) => (
                          <tr key={p.id}>
                            <td>{p.id}</td>
                            <td style={{ fontWeight: 500 }}>{p.nome}</td>
                            <td>{p.idade} anos</td>
                            <td style={{ textAlign: 'center' }}>
                              <button 
                                className="btn-delete"
                                onClick={() => p.id !== undefined && handleDeletePessoa(p.id)}
                                title="Excluir pessoa e transações em cascata"
                              >
                                <Trash2 size={16} />
                              </button>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <div className="empty-state">
                    <Users className="empty-state-icon" size={48} />
                    <div className="empty-state-title">Nenhuma pessoa registrada</div>
                    <div className="empty-state-desc">Utilize o painel ao lado para cadastrar a primeira pessoa física no sistema.</div>
                  </div>
                )}
              </div>

              <div className="ui-panel">
                <h3 className="section-title">
                  <PlusCircle size={20} />
                  Nova Pessoa
                </h3>
                <form onSubmit={handleCreatePessoa}>
                  <div className="form-group">
                    <label className="form-label" htmlFor="pessoa-nome">Nome Completo</label>
                    <input 
                      type="text" 
                      id="pessoa-nome"
                      className="form-input" 
                      placeholder="Ex: João Silva"
                      value={pessoaForm.nome}
                      onChange={(e) => setPessoaForm({ ...pessoaForm, nome: e.target.value })}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label" htmlFor="pessoa-idade">Idade (anos)</label>
                    <input 
                      type="number" 
                      id="pessoa-idade"
                      className="form-input" 
                      placeholder="Ex: 28"
                      value={pessoaForm.idade}
                      onChange={(e) => setPessoaForm({ ...pessoaForm, idade: e.target.value })}
                      required
                    />
                  </div>
                  <button type="submit" className="btn-submit">
                    <PlusCircle size={18} />
                    Cadastrar Pessoa
                  </button>
                </form>
              </div>
            </div>
          )}

          {activeTab === 'transacoes' && (
            <div className="grid-content">
              <div className="ui-panel">
                <h3 className="section-title">
                  <DollarSign size={20} />
                  Histórico de Transações
                </h3>
                {transacoes.length > 0 ? (
                  <div className="table-responsive">
                    <table className="modern-table">
                      <thead>
                        <tr>
                          <th>Descrição</th>
                          <th>Responsável</th>
                          <th>Tipo</th>
                          <th>Valor</th>
                        </tr>
                      </thead>
                      <tbody>
                        {transacoes.map((t) => (
                          <tr key={t.id}>
                            <td style={{ fontWeight: 500 }}>{t.descricao}</td>
                            <td>{t.pessoa?.nome ?? `ID: ${t.personId}`}</td>
                            <td>
                              <span className={`badge ${t.tipo === TipoTransacao.Receita ? 'receita' : 'despesa'}`}>
                                {t.tipo === TipoTransacao.Receita ? 'Receita' : 'Despesa'}
                              </span>
                            </td>
                            <td className={t.tipo === TipoTransacao.Receita ? 'text-success' : 'text-danger'}>
                              {formatCurrency(t.valor)}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <div className="empty-state">
                    <DollarSign className="empty-state-icon" size={48} />
                    <div className="empty-state-title">Nenhuma transação lançada</div>
                    <div className="empty-state-desc">Use o painel de lançamento ao lado para registrar despesas ou receitas.</div>
                  </div>
                )}
              </div>

              <div className="ui-panel">
                <h3 className="section-title">
                  <PlusCircle size={20} />
                  Lançar Transação
                </h3>
                <form onSubmit={handleCreateTransacao}>
                  <div className="form-group">
                    <label className="form-label" htmlFor="transacao-pessoa">Responsável</label>
                    <select 
                      id="transacao-pessoa"
                      className="form-select"
                      value={transacaoForm.personId}
                      onChange={(e) => setTransacaoForm({ ...transacaoForm, personId: e.target.value })}
                      required
                    >
                      <option value="">Selecione uma pessoa...</option>
                      {pessoas.map((p) => (
                        <option key={p.id} value={p.id}>
                          {p.nome} ({p.idade} {p.idade === 1 ? 'ano' : 'anos'})
                        </option>
                      ))}
                    </select>
                  </div>
                  <div className="form-group">
                    <label className="form-label" htmlFor="transacao-descricao">Descrição</label>
                    <input 
                      type="text" 
                      id="transacao-descricao"
                      className="form-input" 
                      placeholder="Ex: Supermercado, Aluguel"
                      value={transacaoForm.descricao}
                      onChange={(e) => setTransacaoForm({ ...transacaoForm, descricao: e.target.value })}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label" htmlFor="transacao-valor">Valor (R$)</label>
                    <input 
                      type="number" 
                      step="0.01"
                      id="transacao-valor"
                      className="form-input" 
                      placeholder="Ex: 150.50"
                      value={transacaoForm.valor}
                      onChange={(e) => setTransacaoForm({ ...transacaoForm, valor: e.target.value })}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label className="form-label" htmlFor="transacao-tipo">Tipo de Transação</label>
                    <select 
                      id="transacao-tipo"
                      className="form-select"
                      value={transacaoForm.tipo}
                      onChange={(e) => setTransacaoForm({ ...transacaoForm, tipo: parseInt(e.target.value, 10) as TipoTransacao })}
                      required
                    >
                      <option value={TipoTransacao.Despesa}>Despesa (Saída)</option>
                      <option value={TipoTransacao.Receita}>Receita (Entrada)</option>
                    </select>
                  </div>
                  <button type="submit" className="btn-submit">
                    <PlusCircle size={18} />
                    Confirmar Lançamento
                  </button>
                </form>
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}

export default App;
