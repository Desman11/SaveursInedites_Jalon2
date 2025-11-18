-- Création du schéma public si nécessaire (en général déjà présent par défaut)
CREATE SCHEMA IF NOT EXISTS public;

-- On définit le schéma public comme schéma par défaut pour les objets créés ensuite
SET search_path TO public;

BEGIN;

-- Nettoyage pour pouvoir rejouer le script sans erreur
DROP TABLE IF EXISTS categories_recettes CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS ingredients_recettes CASCADE;
DROP TABLE IF EXISTS ingredients CASCADE;
DROP TABLE IF EXISTS etapes CASCADE;
DROP TABLE IF EXISTS avis CASCADE;
DROP TABLE IF EXISTS recette CASCADE;
DROP TABLE IF EXISTS utilisateur CASCADE;

-----------------------------------------------------------
-- Table des utilisateurs (comptes de la plateforme)
-----------------------------------------------------------
CREATE TABLE utilisateur (
    id          SERIAL PRIMARY KEY,
    identifiant VARCHAR(20) UNIQUE NOT NULL,
    email       VARCHAR(50) UNIQUE NOT NULL,
    password    VARCHAR(255) NOT NULL,
    role_id     INT NOT NULL
);

-----------------------------------------------------------
-- Table principale des recettes
-----------------------------------------------------------
CREATE TABLE recette (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    temps_preparation INTERVAL NOT NULL,
    temps_cuisson     INTERVAL NOT NULL DEFAULT '00:00:00',
    difficulte INT NOT NULL CHECK (difficulte BETWEEN 1 AND 5),
    photo VARCHAR(100),
    createur INT REFERENCES utilisateur(id) ON DELETE CASCADE
);

-----------------------------------------------------------
-- Autres tables (comme tu les avais)
-----------------------------------------------------------
CREATE TABLE avis (
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    id_utilisateur INT NOT NULL REFERENCES utilisateur(id) ON DELETE CASCADE,
    note INT NOT NULL CHECK (note BETWEEN 1 AND 5),
    commentaire VARCHAR(500),
    PRIMARY KEY (id_recette, id_utilisateur)
);

CREATE TABLE etapes (
    numero INT NOT NULL,
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    texte VARCHAR(500) NOT NULL,
    PRIMARY KEY (numero, id_recette)
);

CREATE TABLE ingredients (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE ingredients_recettes (
    id_ingredient INT NOT NULL REFERENCES ingredients(id) ON DELETE CASCADE,
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    quantite VARCHAR(40) NOT NULL,
    PRIMARY KEY (id_ingredient, id_recette)
);

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE categories_recettes (
    id_categorie INT NOT NULL REFERENCES categories(id) ON DELETE CASCADE,
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    PRIMARY KEY (id_categorie, id_recette)
);

-----------------------------------------------------------
-- Données de test : utilisateur admin
-----------------------------------------------------------

-- À FAIRE UNE FOIS DANS UN PETIT PROGRAMME C# :
-- var hash = BCrypt.Net.BCrypt.HashPassword("admin");
-- Console.WriteLine(hash);
-- puis coller le hash obtenu à la place de 'HASH_BCRYPT_ADMIN_A_REMPLACER'

INSERT INTO utilisateur (identifiant, email, password, role_id)
VALUES (
    'admin',
    'admin@example.com',
    '$2b$10$pSg.UpAtwmQ10uBmFs2W0u/cKgnA/hgfopXEXebFkYKMXNyfYvbby',
    1
);


-----------------------------------------------------------
-- Données de test : 6 recettes
-----------------------------------------------------------

INSERT INTO recette (nom, temps_preparation, temps_cuisson, difficulte, photo, createur)
VALUES 
    ('nom_1','00:00:00','00:00:00',3,'photo_1', NULL),
    ('nom_2','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_3','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_4','00:00:00','00:00:00',5,'photo_1', NULL),
    ('nom_5','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_6','00:00:00','00:00:00',3,'photo_1', NULL);

COMMIT;
