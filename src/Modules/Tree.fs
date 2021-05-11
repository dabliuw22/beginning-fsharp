module Tree

type Tree<'a> =
    | Empty
    | Node of value: 'a * left: Tree<'a> * right: Tree<'a>

module Tree =
    let tree : Tree<int> =
        Node(20, Node(9, Node(4, Node(2, Empty, Empty), Empty), Node(10, Empty, Empty)), Empty)

let rec contains<'a when 'a: comparison> (item: 'a) (tree: Tree<'a>) : bool =
    match tree with
    | Empty -> false
    | Node (v, l, r) ->
        if v = item then true
        elif item < v then contains item l
        else contains v r
