

export const goNext = (router) => {
    const query = router.currentRoute.value.query;

    const next = query.next;

    if (next) {
        delete query.next;

        router.push({ name: next, params: query });
        return;
    }

    router.push({ name: "home" });
};