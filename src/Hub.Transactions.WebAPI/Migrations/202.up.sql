DO $$ 
    BEGIN
        BEGIN
			ALTER TABLE cs_rpt_txn ADD COLUMN global_id varchar(100);
        EXCEPTION
            WHEN duplicate_column THEN RAISE NOTICE 'column global_id already exists in cs_rpt_txn.';
        END;
        BEGIN
			ALTER TABLE cs_rpt_txn ADD COLUMN global_name varchar(100);
        EXCEPTION
            WHEN duplicate_column THEN RAISE NOTICE 'column global_name already exists in cs_rpt_txn.';
        END;
        BEGIN
			ALTER TABLE cs_rpt_txn ADD COLUMN original_payment_id varchar(50);
        EXCEPTION
            WHEN duplicate_column THEN RAISE NOTICE 'column original_payment_id already exists in cs_rpt_txn.';
        END;
    END;
$$