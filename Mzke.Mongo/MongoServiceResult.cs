using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Mzke.Mongo
{
    public class MongoServiceResult<T>
    {
        private MongoServiceResultModel<T> _model;

        public MongoServiceResult()
        {
            _model = new MongoServiceResultModel<T> { Sucesso = true };
        }

        public MongoServiceResult(MongoServiceResultModel<T> resultado)
        {
            _model = resultado;
        }

        public MongoServiceResult(Exception ex)
        {
            _model = new MongoServiceResultModel<T> { Sucesso = false };
            Add(ex.GetBaseException().ToString());
        }

        public MongoServiceResult(string erro)
        {
            _model = new MongoServiceResultModel<T> { Sucesso = false };
            Add(erro);
        }

        public void Add(string mensagem)
        {
            _model.Mensagens.Add(mensagem);
        }

        public void Fracasso(string erro)
        {
            _model.Sucesso = false;
            Add(erro);
        }

        public void Fracasso(Exception ex)
        {
            _model.Sucesso = false;
            Add(ex.GetBaseException().ToString());
        }

        public bool Sucesso
        {
            get { return _model.Sucesso; }
        }

        public MongoServiceResultModel<T> Model         
        {
            get { return _model; }
        }
    }
}
