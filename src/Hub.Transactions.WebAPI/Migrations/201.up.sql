DO $$ 
    BEGIN
        BEGIN
			ALTER TABLE cs_rpt_timestamp ADD COLUMN provider_transaction_number character varying(50);
        EXCEPTION
            WHEN duplicate_column THEN RAISE NOTICE 'column provider_transaction_number already exists in cs_rpt_timestamp.';
        END;
    END;
$$