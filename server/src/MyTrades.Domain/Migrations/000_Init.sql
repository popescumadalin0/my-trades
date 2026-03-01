-- 001_Init.sql

CREATE TABLE IF NOT EXISTS __migrations (
                                            id SERIAL PRIMARY KEY,
                                            script_name VARCHAR(255) NOT NULL UNIQUE,
                                            executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS candles (
                                       id BIGSERIAL PRIMARY KEY,

                                       symbol VARCHAR(20) NOT NULL,
                                       timeframe VARCHAR(10) NOT NULL, -- 1m, 5m, 1h

                                       open_time TIMESTAMPTZ NOT NULL,
                                       close_time TIMESTAMPTZ NOT NULL,

                                       open_price NUMERIC(18,8) NOT NULL,
                                       high_price NUMERIC(18,8) NOT NULL,
                                       low_price NUMERIC(18,8) NOT NULL,
                                       close_price NUMERIC(18,8) NOT NULL,

                                       volume NUMERIC(20,8) NOT NULL,
                                       trade_count INTEGER DEFAULT 0,

                                       created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

                                       CONSTRAINT uq_candle UNIQUE(symbol, timeframe, open_time)
);