**********************************Jeu de combat - "One Ring to Kill them All" - Groupe F******************************

Le jeu a été développé sur Visual Studio 2019 en utilisant NAudio pour les sons, par Diane AVEILLAN, Thibault VINCENT et Enki BACHELIER (Chaque auteur a contribué de façon égale).

************************************************INPUTS********************************************************************

Le jeu demande au joueur uniquement des inputs claviers (lettres, numéros ou juste presser une touche) et renvoie un message personnalisé en cas d'une entrée incorrecte.

************************************************FONCTIONNEMENT DU JEU********************************************************************

Au lancement, le jeu se met en plein écran.

Une histoire s'affiche au début de chaque manche et à la fin de la partie (en cas de victoire).

Le titre s'affiche en haut de la page et une première question est demandée au joueur pour entrer son nom (qui sera utilisé tout au long du jeu).

S'affiche alors les 6 classes de personnage et leur description (PV, Attaque, Spécial). La sélection se fait en indiquant le numéro correspondant au choix de la classe.

On indique ensuite le niveau de difficulté sur une échelle de 3. De même que précédemment, il faut écrire le chiffre correspondant à la difficulté souhaitée.

*************************1ère MANCHE

L'adversaire contre qui on va combattre est dévoilé (avec la description de son attaque spéciale) et les statistiques des deux entités (joueur et IA) sont indiqués (PV, Attaque et le nom du Spécial).

Ensuite, le programme demande au joueur l'action à effectuer : 

- 1 pour l'attaque de base qui va infliger la valeur de notre attaque à l'ennemi.
- 2 pour se défendre : on ne prend aucun dégât (sauf contre une attaque spécial d'un Gymlit).
- 3 pour faire l'attaque spécifique à notre classe.

***********************ATTAQUES SPECIALES
- le Rawrhirrim lance "Rage" : il renvoit tous les dégâts qu'il subit pendant le tour.
- le Erwan lance "Snack" : il regénère 2 points de vie (sans dépasser sa limite maximum de PV).
- le Gymlit lance "Puissance" : il perd 1 point de vie mais inflige son attaque de base +1 à l'adversaire. (Son attaque outrepasse la défense de l'adversaire et lui inflige 2 points de dégâts)
- le Degolas lance "vampire curse" : il vole un PV à l'adversaire. Il régénère donc 1 PV (dans la limite de ses PV max) et l'adversaire subit 1 dégât.
- le Gundalph lance "buuuurn" : il enflamme son adversaire, lui faisant subir 1 dégât pendant 3 tours, cumulable s'il le relance (mais ne peut le cumuler plus de trois fois).
- le Striper lance "critical shot" : il inflige entre 0 et 4 points de dégâts de manière aléatoire.

***********************FIN DE MANCHE

La manche se termine quand l'un des deux joueurs perd tous ses points de vie.
Avant de continuer, les PV finaux et l'issue du combat sont affichées :
- si le joueur a gagné, la suite de l'histoire s'affiche et il passe à la seconde manche.
- s'il a perdu, un message personnalisé s'affiche puis l'écran de défaite et les crédits s'affichent.

*********************2eme MANCHE

Elle se lance avec l'annonce du nouvel adversaire.

On joue avec les PV qui nous restaient à la fin de la première manche.

On choisit une potion parmis 2 qui seront utilisable une unique fois pour le reste de la partie :
- la première permet de regagner tous ses points de vie
- la deuxième permet d'infliger 2 points de dégât à l'adversaire. Si celui-ci se défend il subira quand même les 2 points de dégâts.

La manche suivante se déroule donc de la même manière que la première manche à ceci près que le joueur possède une 4e action pour utiliser sa potion.

L'IA est améliorée par rapport à la première manche selon la difficulté choisie.

La fin de manche affiche les mêmes informations qu'à la fin de la manche précédente.

***********************3eme MANCHE

La troisième manche ne propose plus de choisir une potion. Cependant, nos PV ont été remis au maximum.
L'IA est de nouveau améliorée.

Mêmes éléments pour le déroulement et la fin de cette manche que les manches précédentes.

**********************FIN DE JEU

L'écran de victoire ou défaite s'affiche et, après une entrée du joueur, les crédits s'affichent également.

Une option est proposée au joueur, il peut rejouer ou non.

************************************************NOUVELLES FONCTIONNALITES********************************************************************

- 3 classes supplémentaires (Degolas, Gundalph et Striper).
- plusieurs manches de jeux avec une difficulté croissante + 3 niveaux de difficulté (IA plus intelligente) pour toute la campagne de jeu :
	--> Pour le choix de classe de l'IA : aléatoire pour les manches 1, ainsi que pour toute la campagne de difficulté 1.
    	Au niveau 2, la classe de personnage choisie pour la manche 2 se base sur l'action la plus jouée par le joueur à la manche 1.
    	Pour la manche 3 et également pour le  niveau de difficulté 3 (manche 2 et 3), on indique directement une classe adverse à combattre en fonction du tableau de statistiques des réussites.
	--> Pour le choix des actions de l'IA : aléatoire pour les manche 1 et pour toute la campagne de difficulté 1.
    	Pour la difficulté 2 manches 2 et 3, et difficulté 3 manche 2 : l'IA a 3 chances sur 5 de choisir de faire ce qui contre le mieux l'action la plus jouée par le joueur aux manches précédentes.
    	Pour la difficulté 3 manche 3 : l'IA a 3 chances sur 4 de choisir de faire ce qui contre le mieux l'action choisie par le joueur pour le tour en cours.

- les statistiques des personnages à l'aide d'un mode de simulation (actions aléatoires). Le tableau de tests ainsi obtenu a permis d'ajuster l'équilibrage entre les personnages.
- visuel : ajout de titres, affichage des choix à effectuer, affichage des classes de personnage en jeu, affichage des actions réalisées par les deux joueurs, visuel de fin de manche, visuel de fin de partie, crédits.
- audio : son au lancement du jeu, son au lancement du combat : 1ère manche, son différent à la 2e et 3e manche, son de fin de partie.
- ajout de potions.

************************************************DIFFICULTES RESOLUES********************************************************************
affichage du titre en fonction de la taille de la fenêtre

Live Share : problèmes d'affichage
Confusion avec les parenthèses car pas de classes pour tout ranger
Son : afficher les effets en même temps que la musique de fond
Buuuuurn non réinitialisé entre deux manches
Gestion des entrées incorrectes
Cas où le personnage IA == le personnage joueur
Les potions n'avaient pas d'effets
Réutilisation de morceaux de codes dans les fonctions : attention à re-déclarer les variables

************************************************AMELIORATIONS FUTURES********************************************************************

- Davantage de musiques
- Davantage de potions
- Implanter un moteur graphique plus interactif
- Rajouter des énigmes (afin de coller avec le personnage de Gollum)
- Rajouter un menu interactif (Play, Credits et Quit)

************************************************REMERCIEMENTS********************************************************************
- à Pauline, Bao, Léo (P2) et Ydris pour avoir fait les play tests
- 