# DanskeBank-back-end-homework
### Installation
```sh
$ git clone https://github.com/Justas12/DanskeBank-back-end-homework.git
$ cd DanskeBank-back-end-homework/homework
```
Open .sln file with VS code and run the program

### Usage
<pre>
- GET    http://localhost:8000                      -->   View all storage items
- GET    http://localhost/todos/?from=a&count=b     -->   View storage items starting from a up to b
- POST   http://localhost/:8000                     -->   Write to the storage (items must be provided in request body)
</pre>

### Examples
#### POST Request

```
[
  [1, 3, 5, 8, 9, 2, 6, 7, 6, 8, 9],
  [1, 0, 0]
]
```

#### POST Response

```
[
  {
    "Arr": [
      1,
      3,
      5,
      8,
      9,
      2,
      6,
      7,
      6,
      8,
      9
    ],
    "Path": [
      1,
      3,
      9,
      9
    ],
    "Winnable": true
  },
  {
    "Arr": [
      1,
      0,
      0
    ],
    "Path": [],
    "Winnable": false
  }
]
```
