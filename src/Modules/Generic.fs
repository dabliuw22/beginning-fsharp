module Generic

type Maybe<'T> =
    | Just of 'T
    | Nothing

module Maybe =
    let inline fromMaybe (maybe: Maybe<'T>) (other: 'T) : 'T =
        match maybe with
        | Just v -> v
        | _ -> other

    let inline just<'T when 'T: struct> (value) : Maybe<'T> = Just value

    let nothing<'T> : Maybe<'T> = Nothing
