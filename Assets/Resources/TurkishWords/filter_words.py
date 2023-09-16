path = "words.txt"
target_path = "turkish_words.txt"

with open(path, "r", encoding="utf-8") as f:
    lines = [line.strip() for line in f.readlines()]


lines = [line for line in lines if len(line) > 2]
lines = [line for line in lines if " " not in line]
lines = [line for line in lines if not line[0].isupper()]

cursed_words = [
    "sik",
    "sok",
    "taşak",
    "göt",
    "orospu",
    "yarak",
    "fahişe",
    "dürzü",
    "salak",
    "seks",
    "kaltak",
    "pezevenk",
    "puşt",
    "abaza",
    "aşüfte",
    "anüs",
    "jigolo",
    "ibne",
    "lavuk",
    "dallama",
    "dalyarak",
    "keriz",
    "kıç",
    "dangalak",
    "bok",
    "boşalma",
    "şorolo",
    "dığa",
    "kavat",
    "bombalanmak",
    "sakso",
    "sıçma",
    "cünüp",
    "cenabet",
    "hayız",
    "esrar",
    "eroin",
    "erotik",
    "erotizm",
    "oynaş",
    "iktidarsız",
    "genelev",
    "kerhane",
    "lohusa",
    "döl",
    "dikiz",
    "dill",
    "fagot",
    "mastürbasyon",
    "afyon",
    "allah",
    "metres",
    "metroseksüel",
    "kancık",
    "menopoz",
    "andropoz",
    "düdükleme",
    "tecavüz",
    "taciz",
    "düzülme",
    "fort",
    "ablacı",
    "ensest", 
    "soyunma", 
    "gerdek", 
    "godoş", 
    "gebeş", 
    "kodaman", 
    "ayakçı", 
    "fantezi", 
    "cücük", 
    "namus", 
    "ırz", 
    "hafifmeşrep", 
    "oğlan", 
    "nüd", 
    "meme",
    "kızmemesi",
    "köpekmemesi",
    "tavşanmemesi",
    "lavman", 
    "geyşa", 
    "gedilmek", 
    "kaka", 
    "gedme", 
    "şirret", 
    "şehv", 
    "kaypak", 
    "şerefsiz", 
    "bakir", 
    "çiftleşme", 
    "avanak", 
    "attırmak", 
    "damping", 
    "çıplak", 
]


lines = {line for line in lines for cw in cursed_words if not line.startswith(cw)}

newLines = set()
for line in lines:
    if all(not line.startswith(cw) for cw in cursed_words):
        newLines.add(line)


with open(target_path, "w", encoding="utf-8") as f:
    f.writelines(sorted(list(map(lambda x: x+'\n', newLines))))

