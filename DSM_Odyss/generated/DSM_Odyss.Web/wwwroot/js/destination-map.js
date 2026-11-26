(function () {
    let countryDataPromise;

    function normalizeCountryName(value) {
        return String(value || "")
            .normalize("NFD")
            .replace(/[\u0300-\u036f]/g, "")
            .toLowerCase()
            .replace(/[^a-z0-9]+/g, " ")
            .trim();
    }

    function addCountryAlias(index, alias, feature) {
        const normalized = normalizeCountryName(alias);
        if (normalized) index.set(normalized, feature);
    }

    function createCountryIndex(features) {
        const index = new Map();
        const spanishNames = typeof Intl.DisplayNames === "function"
            ? new Intl.DisplayNames(["es"], { type: "region" })
            : null;
        const englishNames = typeof Intl.DisplayNames === "function"
            ? new Intl.DisplayNames(["en"], { type: "region" })
            : null;

        features.forEach(function (feature) {
            const properties = feature.properties || {};
            [
                properties.NAME_ES,
                properties.ADMIN,
                properties.NAME,
                properties.NAME_LONG,
                properties.FORMAL_EN
            ].forEach(name => addCountryAlias(index, name, feature));

            [properties.ISO_A2, properties.ISO_A2_EH].forEach(function (code) {
                if (!/^[A-Z]{2}$/.test(code || "")) return;
                addCountryAlias(index, code, feature);
                if (spanishNames) addCountryAlias(index, spanishNames.of(code), feature);
                if (englishNames) addCountryAlias(index, englishNames.of(code), feature);
            });

            [properties.ISO_N3, properties.ISO_N3_EH].forEach(function (code) {
                if (!/^\d{3}$/.test(code || "")) return;
                if (spanishNames) addCountryAlias(index, spanishNames.of(code), feature);
                if (englishNames) addCountryAlias(index, englishNames.of(code), feature);
            });
        });

        const aliasesByIso = {
            USA: ["USA", "EEUU", "EE. UU.", "Estados Unidos", "Estados Unidos de América"],
            GBR: ["UK", "Gran Bretaña", "Reino Unido"],
            RUS: ["Rusia"],
            NLD: ["Holanda"],
            CIV: ["Costa de Marfil"]
        };
        Object.keys(aliasesByIso).forEach(function (iso) {
            const feature = features.find(item => item.properties?.ISO_A3 === iso);
            if (feature) aliasesByIso[iso].forEach(alias => addCountryAlias(index, alias, feature));
        });

        return index;
    }

    function loadCountryData(url) {
        if (!countryDataPromise) {
            countryDataPromise = fetch(url)
                .then(function (response) {
                    if (!response.ok) throw new Error(`Country map request failed: ${response.status}`);
                    return response.json();
                })
                .then(function (data) {
                    if (!Array.isArray(data.features)) throw new Error("Country map data is invalid.");
                    return data;
                });
        }
        return countryDataPromise;
    }

    function createMap(widget) {
        const canvas = widget.querySelector(".destination-map-canvas");
        const status = widget.querySelector(".destination-map-status");
        if (!canvas || !status || !window.L) {
            if (status) status.textContent = "No se pudo cargar el mapa. Comprueba tu conexión e inténtalo de nuevo.";
            return null;
        }

        const map = L.map(canvas, { scrollWheelZoom: false });

        const emptyMessage = widget.dataset.destinationMap === "profile"
            ? "Aún no tienes países de viaje para mostrar."
            : "Selecciona un país para marcarlo en el mapa.";
        return {
            map: map,
            status: status,
            url: widget.dataset.countryMapUrl,
            layer: null,
            version: 0,
            emptyMessage: emptyMessage
        };
    }

    function countryName(feature) {
        const properties = feature.properties || {};
        return properties.NAME_ES || properties.ADMIN || properties.NAME || "País";
    }

    async function renderCountries(state, locations) {
        if (!state) return;

        const version = ++state.version;
        if (state.layer) {
            state.layer.remove();
            state.layer = null;
        }

        let countryData;
        try {
            countryData = await loadCountryData(state.url);
        } catch (error) {
            console.error("No se pudo cargar el mapa de países.", error);
            state.status.textContent = "No se pudo cargar el mapa de países. Comprueba tu conexión e inténtalo de nuevo.";
            return;
        }
        if (version !== state.version) return;

        const countryIndex = createCountryIndex(countryData.features);
        const selectedNames = new Set(
            locations.map(location => location.country.trim()).filter(Boolean)
        );
        const selectedFeatures = new Set();
        const notRecognized = [];
        selectedNames.forEach(function (name) {
            const feature = countryIndex.get(normalizeCountryName(name));
            if (feature) selectedFeatures.add(feature);
            else notRecognized.push(name);
        });

        state.layer = L.geoJSON(countryData, {
            style: function (feature) {
                const selected = selectedFeatures.has(feature);
                return {
                    color: "#343a40",
                    weight: 0.65,
                    fillColor: selected ? "#111111" : "#ffffff",
                    fillOpacity: 1
                };
            },
            onEachFeature: function (feature, layer) {
                const selected = selectedFeatures.has(feature);
                const label = countryName(feature);
                layer.bindTooltip(label);
                const popup = document.createElement("span");
                popup.textContent = selected ? `${label}: destino de tu viaje` : label;
                layer.bindPopup(popup);

                layer.on({
                    mouseover: function (event) {
                        event.target.setStyle({
                            color: "#000000",
                            weight: 1.5,
                            fillColor: selected ? "#111111" : "#d9d9d9"
                        });
                    },
                    mouseout: function (event) {
                        state.layer.resetStyle(event.target);
                    }
                });
            }
        }).addTo(state.map);

        const bounds = state.layer.getBounds();
        if (bounds.isValid()) state.map.fitBounds(bounds, { padding: [8, 8], maxZoom: 2 });

        if (!selectedNames.size) {
            state.status.textContent = state.emptyMessage;
            return;
        }

        const parts = [`${selectedFeatures.size} ${selectedFeatures.size === 1 ? "país marcado" : "países marcados"}`];
        if (notRecognized.length) parts.push(`No se reconocieron: ${notRecognized.join(", ")}`);
        state.status.textContent = parts.join(". ") + ".";
    }

    function initProfileMap(widget) {
        const state = createMap(widget);
        if (!state) return;

        const locations = Array.from(document.querySelectorAll("[data-map-countries] [data-country]"))
            .map(item => ({ country: item.dataset.country }));
        renderCountries(state, locations);
    }

    function getCardLocation(card) {
        const selector = card.querySelector(".select-destino-catalogo");
        if (selector && selector.value) {
            const label = selector.options[selector.selectedIndex].textContent.trim();
            if (label.startsWith("(") && label.endsWith(")"))
                return { country: label.slice(1, -1).trim() };

            const separator = label.lastIndexOf(" (");
            if (separator >= 0 && label.endsWith(")")) {
                return { country: label.slice(separator + 2, -1).trim() };
            }
            return { country: label };
        }

        return { country: card.querySelector(".input-destino-pais")?.value.trim() || "" };
    }

    function initTripForm(widget) {
        if (!widget) return;
        const state = createMap(widget);
        const container = widget.closest("#collapseDestinos")?.querySelector("#destinos-container");
        if (!state || !container) return;

        let updateTimeout;
        const refresh = function () {
            clearTimeout(updateTimeout);
            updateTimeout = setTimeout(function () {
                const locations = Array.from(container.querySelectorAll(".destino-card")).map(getCardLocation);
                renderCountries(state, locations);
            }, 650);
        };

        container.addEventListener("change", refresh);
        new MutationObserver(refresh).observe(container, { childList: true });

        document.addEventListener("shown.bs.collapse", function (event) {
            if (event.target.contains(widget)) state.map.invalidateSize();
        });

        refresh();
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll('[data-destination-map="profile"]').forEach(initProfileMap);
    });

    window.DSMDestinationMap = { initTripForm: initTripForm };
})();
