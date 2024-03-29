-- Table: public.contas

-- DROP TABLE IF EXISTS public.contas;

CREATE TABLE IF NOT EXISTS public.contas
(
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    cliente_id integer NOT NULL,
    limite integer NOT NULL,
    saldo integer NOT NULL,
    CONSTRAINT pk_clientes PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS idx_contas_cliente_id ON public.contas(cliente_id);

-- Table: public.Transacoes

-- DROP TABLE IF EXISTS public.transacoes;

CREATE TABLE IF NOT EXISTS public.transacoes
(
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    conta_id integer NOT NULL,
    tipo character(1) COLLATE pg_catalog."default" NOT NULL,
    descricao text COLLATE pg_catalog."default" NOT NULL,
    valor integer NOT NULL,
    realizada_em timestamp with time zone NOT NULL,
    CONSTRAINT pk_transacoes PRIMARY KEY (id)
);
CREATE INDEX IF NOT EXISTS idx_transacoes_conta_id ON public.transacoes(conta_id);


DELETE FROM public.contas;
DELETE FROM public.transacoes;

INSERT INTO public.contas(cliente_id, limite, saldo)
	VALUES 
	(1, 100000, 0),
	(2, 80000, 0),
	(3, 1000000, 0),
	(4, 10000000, 0),
	(5, 500000, 0);

