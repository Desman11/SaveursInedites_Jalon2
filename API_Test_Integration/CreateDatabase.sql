-- Création du schéma (optionnel ici, public existe déjà)
CREATE SCHEMA IF NOT EXISTS public;

SET search_path TO public;

BEGIN;

-- (Optionnel en dev) Nettoyage pour rejouer le script sans erreur
DROP TABLE IF EXISTS categories_recettes CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS ingredients_recettes CASCADE;
DROP TABLE IF EXISTS ingredients CASCADE;
DROP TABLE IF EXISTS etapes CASCADE;
DROP TABLE IF EXISTS avis CASCADE;
DROP TABLE IF EXISTS recette CASCADE;
DROP TABLE IF EXISTS utilisateurs CASCADE;

-- Création des tables

CREATE TABLE utilisateurs (
    id SERIAL PRIMARY KEY,
    identifiant VARCHAR(20) UNIQUE NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE recette (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    temps_preparation INTERVAL NOT NULL,
    temps_cuisson INTERVAL NOT NULL DEFAULT '00:00:00',
    difficulte INT NOT NULL CHECK (difficulte BETWEEN 1 AND 5),
    photo VARCHAR(100),
    createur INT REFERENCES utilisateurs(id) ON DELETE CASCADE
);

CREATE TABLE avis (
    id_recette INT NOT NULL REFERENCES recette(id) ON DELETE CASCADE,
    id_utilisateur INT NOT NULL REFERENCES utilisateurs(id) ON DELETE CASCADE,
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

-- Données de test : 6 recettes
INSERT INTO recette (nom, temps_preparation, temps_cuisson, difficulte, photo, createur)
VALUES 
    ('nom_1','00:00:00','00:00:00',3,'photo_1', NULL),
    ('nom_2','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_3','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_4','00:00:00','00:00:00',5,'photo_1', NULL),
    ('nom_5','00:00:00','00:00:00',2,'photo_1', NULL),
    ('nom_6','00:00:00','00:00:00',3,'photo_1', NULL);

COMMIT;
