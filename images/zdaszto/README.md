# /login
## ?username=USERNAME&password=PASSWORD

przy poprawnym zalogowaniu zwraca:
```
TOKEN: ef416b77dcc82b699c39356e8019262e53c16e39c1f3a1d69042beb0369fde61
```

przy podaniu niepoprawnego hasla:
```
Invalid password
```

przy podaniu nieistniejacego uzytkownika:
```
No user found
```



<br><br>
# /signup
## ?email=EMAIL&username=USERNAME&password=PASSWORD

przy poprawnym utworzeniu uzytkownika:
```
User created
```

przy podaniu username ktory juz istnieje:
```
User exists
```



<br><br>
# /question/image/X.json
## ?token=TOKEN

zamiast X numer pytania z bazy danych
|     X     | egzamin |
|-----------|---------|
| 1-624     | inf02   |
| 625-1743  | inf03   |
| 1744-2160 | inf04   |

przy wskazaniu pytania ktore posiada plik:
* zwraca plik png/jpg/mp4

przy wskazaniu pytania bez pliku:
```
No media
```


<br><br>
# /question/X.json
## ?token=TOKEN

przy wskazaniu prawidlego pytania:
```
[
  1, # id pytania
  "1. Układy sekwencyjne zbudowane z zespołu przerzutników, najczęściej synchronicznych typu D, służące do\r\nprzechowywania danych, to", # pytanie
  "ABCD", # typ pytania
  1, # poziom trudnosci (wszedzie jest 1)
  "{\"A\": \"bramki\", \"B\": \"kodery\", \"C\": \"rejestry\", \"D\": \"dekodery\"}", # odpowiedzi
  "C", # poprawna odpowiedz
  null, # sciezka do obrazka, jesli nie jest null trzeba wyslac zapytanie do /question/image/X.json
  "2025-10-25 20:37:17", # data utworzenia
  null # data usuniecia
]
```
przy wskazaniu nieistniejacego pytania:
```Invalid question ID```

<br><br>
# /question/random/X.json
## ?token=TOKEN

przy poprawnym id zestawu pytan:
* zwraca pytanie jak /question/X.json

przy nieprawidlowym id zestawu pytan:
```
Invalid collection ID
```



<br><br>
# /collections.json
## ?token=TOKEN

zwraca liste zestawow pytan:
```
[
  [
    1, # id zestawu pytan
    "INF.02", # nazwa
    "Administracja i eksploatacja systemów komputerowych, urządzeń peryferyjnych i lokalnych sieci komputerowych dla zawodu technika informatyka", # opis
    "2025-10-25 20:45:15", # data utworzenia
    null # data usuniecia
  ],
  [
    2,
    "INF.03",
    "Tworzenie i administrowanie stronami i aplikacjami internetowymi oraz bazami danych w zawodzie technika informatyka i technika programisty",
    "2025-10-25 20:45:15",
    null
  ],
  [
    3,
    "INF.04",
    "Projektowanie, programowanie i testowanie aplikacji, część zawodu technik programista",
    "2025-10-25 20:45:56",
    null
  ]
]
```


<br><br>
# /collections/X.json
## ?token=TOKEN

przy poprawnym id zestawu pytan:
```
[1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
```

przy nieistniejacym id:
```
[]
```