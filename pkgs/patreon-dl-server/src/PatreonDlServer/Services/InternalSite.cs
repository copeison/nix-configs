namespace PatreonDlServer.Services;

public static class InternalSite
{
    public const string IndexHtml = """
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Patreon Browse Server</title>
  <style>
    :root {
      --bg: #181a1b;
      --panel: #1c1f22;
      --panel-2: #202428;
      --panel-3: #15181a;
      --line: #2d3136;
      --text: #f2f4f7;
      --muted: #a2a9b3;
      --accent: #1f7ae0;
      --accent-2: #5fa7ff;
      --shadow: 0 16px 40px rgba(0, 0, 0, 0.35);
      --sidebar-width: 336px;
    }

    :root[data-theme="light"] {
      --bg: #eef2f6;
      --panel: #ffffff;
      --panel-2: #f8fafc;
      --panel-3: #edf2f7;
      --line: #d8e0ea;
      --text: #18212b;
      --muted: #617080;
      --accent: #1f7ae0;
      --accent-2: #1f7ae0;
      --shadow: 0 16px 40px rgba(18, 33, 51, 0.12);
    }

    * { box-sizing: border-box; }

    html, body {
      margin: 0;
      width: 100%;
      min-height: 100%;
      overflow-x: hidden;
      overflow-y: auto;
      font-family: "Segoe UI", "Noto Sans", sans-serif;
      background: var(--bg);
      color: var(--text);
    }

    button, input {
      font: inherit;
    }

    .app {
      width: 100vw;
      min-height: 100vh;
      background: var(--bg);
    }

    .sidebar {
      position: fixed;
      top: 0;
      left: 0;
      width: var(--sidebar-width);
      height: 100vh;
      border-right: 1px solid var(--line);
      background: var(--panel-3);
      display: flex;
      flex-direction: column;
      min-height: 0;
      overflow: hidden;
      z-index: 30;
      transition: transform 180ms ease;
    }

    .sidebar-section {
      flex: 1 1 auto;
      padding: 0.875rem 0.75rem 0;
      min-height: 0;
      overflow-y: auto;
      overflow-x: hidden;
      overscroll-behavior: contain;
      scrollbar-gutter: stable;
    }

    .sidebar-heading {
      margin: 0 0 0.85rem;
      color: var(--text);
      font-size: 0.95rem;
      font-weight: 700;
      letter-spacing: 0.01em;
    }

    .creator-list {
      display: grid;
      gap: 0.35rem;
    }

    .creator-status {
      margin-bottom: 0.75rem;
      color: var(--muted);
      font-size: 0.85rem;
      line-height: 1.45;
    }

    .creator-item {
      appearance: none;
      width: 100%;
      border: 0;
      background: transparent;
      display: grid;
      grid-template-columns: 28px 1fr;
      align-items: center;
      gap: 0.65rem;
      padding: 0.45rem 0.35rem;
      border-radius: 8px;
      cursor: pointer;
      color: var(--text);
      text-align: left;
    }

    .creator-item:hover,
    .creator-item.active {
      background: color-mix(in srgb, var(--accent) 12%, transparent);
    }

    .creator-avatar {
      width: 28px;
      height: 28px;
      border-radius: 4px;
      object-fit: cover;
      background: var(--panel-2);
      border: 1px solid var(--line);
    }

    .creator-avatar.placeholder {
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 0.72rem;
      font-weight: 700;
      color: var(--text);
      background: color-mix(in srgb, var(--accent) 22%, var(--panel-2));
      text-transform: uppercase;
    }

    .creator-name {
      font-size: 0.95rem;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    .main {
      margin-left: var(--sidebar-width);
      width: calc(100vw - var(--sidebar-width));
      min-height: 100vh;
      min-width: 0;
      min-height: 0;
      display: flex;
      flex-direction: column;
      overflow: visible;
    }

    .toolbar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 1rem;
      padding: 0.75rem 1rem;
      border-bottom: 1px solid var(--line);
      background: var(--panel);
    }

    .toolbar-start {
      display: flex;
      align-items: center;
      gap: 0.85rem;
      min-width: 0;
    }

    .toolbar-title {
      font-size: 0.95rem;
      font-weight: 700;
      color: var(--muted);
      letter-spacing: 0.02em;
      text-transform: uppercase;
    }

    .menu-button {
      display: none;
      appearance: none;
      border: 1px solid var(--line);
      border-radius: 10px;
      background: transparent;
      color: var(--text);
      width: 2.75rem;
      height: 2.75rem;
      cursor: pointer;
      flex: 0 0 auto;
      align-items: center;
      justify-content: center;
      font-size: 1.2rem;
      line-height: 1;
    }

    .menu-button:hover {
      background: color-mix(in srgb, var(--accent) 10%, transparent);
    }

    .toolbar-actions {
      display: flex;
      align-items: center;
      gap: 0.65rem;
    }

    .toolbar-button {
      appearance: none;
      border: 1px solid var(--line);
      border-radius: 999px;
      background: transparent;
      color: var(--text);
      padding: 0.5rem 0.9rem;
      cursor: pointer;
    }

    .toolbar-button:hover {
      background: color-mix(in srgb, var(--accent) 10%, transparent);
      border-color: color-mix(in srgb, var(--accent) 35%, var(--line));
    }

    .hero {
      position: relative;
      background: var(--panel-3);
      border-bottom: 1px solid #665947;
      overflow: hidden;
    }

    :root[data-theme="light"] .hero {
      border-bottom-color: var(--line);
    }

    .hero-banner {
      position: relative;
      height: 188px;
      background: linear-gradient(135deg, #718ce0, #9db2ff);
      overflow: hidden;
    }

    :root:not([data-theme="light"]) .hero-banner {
      background: linear-gradient(135deg, #293684, #4752bc);
    }

    .hero-cover {
      position: absolute;
      inset: 0;
      width: 100%;
      height: 100%;
      object-fit: cover;
      opacity: 0.92;
    }

    .hero-backdrop {
      position: absolute;
      inset: 0;
      background:
        radial-gradient(circle at 20% 20%, rgba(255,255,255,0.22) 0, rgba(255,255,255,0.08) 18%, transparent 34%),
        radial-gradient(circle at 55% 28%, rgba(255,255,255,0.18) 0, rgba(255,255,255,0.05) 15%, transparent 32%),
        radial-gradient(circle at 88% 10%, rgba(255,255,255,0.24) 0, rgba(255,255,255,0.08) 14%, transparent 28%);
      pointer-events: none;
    }

    .hero-content {
      position: relative;
      z-index: 2;
      display: flex;
      flex-direction: column;
      align-items: center;
      text-align: center;
      padding: 0 2rem 0;
      margin-top: -42px;
    }

    .hero-avatar {
      width: 156px;
      height: 156px;
      border-radius: 12px;
      border: 1px solid rgba(255,255,255,0.2);
      box-shadow: 0 16px 34px rgba(0,0,0,0.45);
      background: var(--panel-2);
      object-fit: cover;
    }

    .hero-title {
      margin: 0.9rem 0 0.35rem;
      font-size: clamp(2rem, 4vw, 3.2rem);
      font-weight: 300;
      letter-spacing: 0.01em;
    }

    .hero-summary {
      margin: 0;
      color: var(--muted);
      font-size: 1rem;
      max-width: 840px;
      overflow: hidden;
      display: -webkit-box;
      -webkit-box-orient: vertical;
      -webkit-line-clamp: 3;
      line-height: 1.45;
    }

    .tabs {
      display: flex;
      justify-content: center;
      flex-wrap: wrap;
      gap: 1.8rem;
      margin-top: 2rem;
      border-bottom: 1px solid #5c5144;
      width: calc(100% + 4rem);
      padding: 0 2rem;
    }

    :root[data-theme="light"] .tabs {
      border-bottom-color: var(--line);
    }

    .tab {
      position: relative;
      appearance: none;
      border: 0;
      background: transparent;
      color: var(--accent-2);
      padding: 0 0 0.85rem;
      cursor: pointer;
      font-size: 0.95rem;
    }

    .tab.active::after {
      content: "";
      position: absolute;
      left: 0;
      right: 0;
      bottom: -1px;
      height: 2px;
      background: var(--accent);
    }

    .content {
      flex: 0 0 auto;
      min-height: auto;
      overflow: visible;
      padding: 1.6rem 2rem 2rem;
    }

    .content-inner {
      width: min(980px, 100%);
      margin: 0 auto;
    }

    .search-row {
      display: grid;
      grid-template-columns: 1fr auto auto;
      gap: 0;
      align-items: stretch;
      margin-bottom: 1rem;
    }

    .search-input {
      width: 100%;
      border: 1px solid var(--line);
      border-right: 0;
      border-radius: 10px 0 0 10px;
      background: transparent;
      color: var(--text);
      padding: 0.9rem 1rem;
      outline: none;
    }

    .search-action,
    .filter-button {
      border: 1px solid var(--accent);
      background: transparent;
      color: var(--accent-2);
      padding: 0 1rem;
      cursor: pointer;
    }

    .search-action {
      background: var(--accent);
      color: white;
      border-radius: 0;
    }

    .filter-button {
      border-radius: 0 10px 10px 0;
      margin-left: 0.65rem;
    }

    .counter {
      margin-bottom: 0.8rem;
      color: var(--muted);
      font-size: 0.95rem;
    }

    .pagination {
      display: flex;
      flex-wrap: wrap;
      align-items: center;
      justify-content: space-between;
      gap: 0.85rem;
      margin-top: 1.15rem;
      color: var(--muted);
      font-size: 0.92rem;
    }

    .pagination-controls {
      display: flex;
      flex-wrap: wrap;
      gap: 0.5rem;
      align-items: center;
    }

    .page-button {
      appearance: none;
      border: 1px solid var(--line);
      border-radius: 10px;
      background: transparent;
      color: var(--text);
      padding: 0.5rem 0.85rem;
      cursor: pointer;
    }

    .page-button:hover:not(:disabled) {
      background: color-mix(in srgb, var(--accent) 10%, transparent);
      border-color: color-mix(in srgb, var(--accent) 35%, var(--line));
    }

    .page-button:disabled {
      opacity: 0.45;
      cursor: default;
    }

    .post-list {
      display: grid;
      gap: 1rem;
    }

    .post-card {
      border-radius: 10px;
      overflow: hidden;
      background: var(--panel-2);
      border: 1px solid var(--line);
      box-shadow: var(--shadow);
    }

    .post-cover {
      width: 100%;
      aspect-ratio: 16 / 7;
      object-fit: cover;
      display: block;
      background: #2b2f34;
    }

    .post-cover-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      grid-template-rows: repeat(2, minmax(0, 1fr));
      gap: 2px;
      aspect-ratio: 16 / 10;
      background: #2b2f34;
      overflow: hidden;
    }

    .post-cover-grid.count-2 {
      grid-template-columns: repeat(2, 1fr);
      grid-template-rows: 1fr;
      aspect-ratio: 16 / 7;
    }

    .post-cover-grid.count-3 {
      grid-template-columns: repeat(2, 1fr);
      grid-template-rows: repeat(2, minmax(0, 1fr));
      aspect-ratio: 16 / 10;
    }

    .post-cover-grid.count-3 .post-cover-tile:nth-child(3) {
      grid-column: 1 / span 2;
    }

    .post-cover-grid.count-4plus {
      grid-template-columns: repeat(2, 1fr);
      grid-template-rows: repeat(2, minmax(0, 1fr));
      aspect-ratio: 16 / 10;
    }

    .post-cover-tile {
      position: relative;
      min-width: 0;
      min-height: 0;
      cursor: pointer;
      background: #2b2f34;
    }

    .post-cover-tile img,
    .post-cover-tile video {
      width: 100%;
      height: 100%;
      object-fit: cover;
      display: block;
      background: #2b2f34;
    }

    .post-cover-more {
      position: absolute;
      inset: 0;
      display: flex;
      align-items: center;
      justify-content: center;
      background: rgba(0, 0, 0, 0.48);
      color: #ffffff;
      font-size: 1.15rem;
      font-weight: 700;
    }

    .post-body {
      padding: 1rem 1rem 1.1rem;
    }

    .post-title {
      margin: 0 0 0.35rem;
      font-size: 1.05rem;
    }

    .post-title-link {
      color: inherit;
      text-decoration: none;
    }

    .post-title-link:hover {
      color: var(--accent-2);
    }

    .post-meta {
      color: var(--muted);
      font-size: 0.9rem;
      margin-bottom: 0.85rem;
    }

    .post-actions {
      display: flex;
      justify-content: flex-end;
    }

    .action-link,
    .primary-button,
    .secondary-button {
      appearance: none;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      border-radius: 999px;
      text-decoration: none;
      font: inherit;
      cursor: pointer;
    }

    .action-link {
      border: 0;
      background: transparent;
      color: var(--accent-2);
      padding: 0;
    }

    .primary-button {
      border: 0;
      background: var(--accent);
      color: white;
      padding: 0.7rem 1rem;
    }

    .secondary-button {
      border: 1px solid var(--line);
      background: transparent;
      color: var(--text);
      padding: 0.7rem 1rem;
    }

    .empty-state,
    .message,
    .article {
      border: 1px solid var(--line);
      border-radius: 12px;
      background: var(--panel-2);
      padding: 1rem;
    }

    .article h2 {
      margin: 0 0 0.35rem;
      font-size: 1.7rem;
      font-weight: 400;
    }

    .article-meta {
      color: var(--muted);
      margin-bottom: 1rem;
      font-size: 0.95rem;
    }

    .info-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 0.85rem;
      margin-bottom: 1rem;
    }

    .info-card {
      border: 1px solid var(--line);
      border-radius: 12px;
      background: var(--panel-2);
      padding: 0.9rem 1rem;
    }

    .info-label {
      color: var(--muted);
      font-size: 0.82rem;
      text-transform: uppercase;
      letter-spacing: 0.04em;
      margin-bottom: 0.4rem;
    }

    .info-value {
      font-size: 1rem;
      line-height: 1.35;
      word-break: break-word;
    }

    .media-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
      gap: 1rem;
    }

    .media-card {
      border: 1px solid var(--line);
      border-radius: 12px;
      overflow: hidden;
      background: var(--panel-2);
      box-shadow: var(--shadow);
    }

    .media-thumb {
      width: 100%;
      aspect-ratio: 16 / 10;
      object-fit: cover;
      display: block;
      background: #2b2f34;
    }

    .media-body {
      padding: 0.85rem 0.95rem 1rem;
    }

    .media-title {
      margin: 0 0 0.35rem;
      font-size: 1rem;
      line-height: 1.35;
    }

    .media-subtitle {
      color: var(--muted);
      font-size: 0.9rem;
      margin-bottom: 0.75rem;
    }

    .reward-list {
      display: grid;
      gap: 0.85rem;
      margin-top: 1rem;
    }

    .reward-card {
      border: 1px solid var(--line);
      border-radius: 12px;
      background: var(--panel-2);
      padding: 0.95rem 1rem;
    }

    .collection-header {
      display: grid;
      grid-template-columns: 56px 1fr;
      gap: 0.9rem;
      align-items: center;
      margin-bottom: 0.85rem;
    }

    .collection-icon {
      width: 56px;
      height: 56px;
      border-radius: 12px;
      object-fit: cover;
      border: 1px solid var(--line);
      background: var(--panel-3);
    }

    .collection-icon.placeholder {
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1rem;
      font-weight: 700;
      color: var(--text);
      background: color-mix(in srgb, var(--accent) 22%, var(--panel-2));
      text-transform: uppercase;
    }

    .collection-link {
      color: inherit;
      text-decoration: none;
    }

    .collection-link:hover {
      color: var(--accent-2);
    }

    .reward-title {
      margin: 0 0 0.35rem;
      font-size: 1rem;
    }

    .reward-description {
      color: var(--muted);
      line-height: 1.5;
      white-space: pre-wrap;
    }

    .content-body img {
      max-width: 100%;
      height: auto;
      border-radius: 12px;
    }

    .detail-section {
      margin-top: 1.25rem;
    }

    .detail-section h3 {
      margin: 0 0 0.75rem;
      font-size: 1.05rem;
      font-weight: 600;
    }

    .asset-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
      gap: 0.9rem;
    }

    .asset-card {
      border: 1px solid var(--line);
      border-radius: 12px;
      overflow: hidden;
      background: var(--panel-2);
    }

    .asset-card img,
    .asset-card video,
    .asset-card audio {
      width: 100%;
      display: block;
      background: var(--panel-3);
    }

    .asset-card img[data-viewer-index],
    .asset-card video[data-viewer-index],
    .media-thumb {
      cursor: pointer;
    }

    .asset-card audio {
      padding: 0.75rem;
    }

    .asset-body {
      padding: 0.8rem 0.9rem;
    }

    .attachment-list {
      display: grid;
      gap: 0.75rem;
    }

    .attachment-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 1rem;
      border: 1px solid var(--line);
      border-radius: 12px;
      background: var(--panel-2);
      padding: 0.85rem 0.95rem;
    }

    .attachment-name {
      font-weight: 600;
      word-break: break-word;
    }

    .viewer-overlay {
      position: fixed;
      inset: 0;
      display: none;
      background: rgba(0, 0, 0, 0.96);
      z-index: 90;
      color: #f5f5f5;
    }

    .viewer-overlay.open {
      display: block;
    }

    .viewer-topbar {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 1rem 1.2rem;
      z-index: 2;
    }

    .viewer-counter {
      font-size: 1rem;
      color: rgba(255,255,255,0.78);
    }

    .viewer-actions {
      display: flex;
      align-items: center;
      gap: 0.6rem;
    }

    .viewer-button {
      appearance: none;
      border: 0;
      background: transparent;
      color: rgba(255,255,255,0.78);
      font-size: 1.6rem;
      line-height: 1;
      cursor: pointer;
      padding: 0.2rem 0.35rem;
    }

    .viewer-button:hover {
      color: #ffffff;
    }

    .viewer-stage {
      position: absolute;
      inset: 0;
      display: grid;
      place-items: center;
      padding: 4.5rem 6rem 4rem;
      touch-action: pan-y;
    }

    .viewer-media {
      max-width: min(1400px, 100%);
      max-height: calc(100vh - 9rem);
      display: block;
      object-fit: contain;
      box-shadow: 0 18px 48px rgba(0, 0, 0, 0.35);
    }

    .viewer-nav {
      position: absolute;
      top: 50%;
      transform: translateY(-50%);
      appearance: none;
      border: 0;
      background: rgba(255,255,255,0.08);
      color: #ffffff;
      width: 3rem;
      height: 3rem;
      border-radius: 999px;
      font-size: 1.8rem;
      cursor: pointer;
      z-index: 2;
    }

    .viewer-nav:hover {
      background: rgba(255,255,255,0.16);
    }

    .viewer-nav.prev {
      left: 1.25rem;
    }

    .viewer-nav.next {
      right: 1.25rem;
    }

    .viewer-caption {
      position: absolute;
      left: 50%;
      bottom: 1.25rem;
      transform: translateX(-50%);
      max-width: min(960px, calc(100vw - 3rem));
      text-align: center;
      color: rgba(255,255,255,0.88);
      font-size: 0.95rem;
      font-weight: 600;
    }

    .message.error {
      border-color: #6a3938;
    }

    .message.success {
      border-color: #365c45;
    }

    .overlay {
      position: fixed;
      inset: 0;
      display: none;
      align-items: center;
      justify-content: center;
      background: rgba(0, 0, 0, 0.56);
      padding: 1rem;
      z-index: 40;
    }

    .overlay.open {
      display: flex;
    }

    .modal {
      width: min(620px, 100%);
      max-height: min(90vh, 760px);
      overflow: auto;
      border: 1px solid var(--line);
      border-radius: 16px;
      background: var(--panel);
      box-shadow: var(--shadow);
    }

    .modal-header {
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
      gap: 1rem;
      padding: 1rem 1rem 0.8rem;
      border-bottom: 1px solid var(--line);
      background: var(--panel-2);
      position: sticky;
      top: 0;
      z-index: 1;
    }

    .modal-body {
      padding: 1rem;
    }

    .filter-section {
      display: grid;
      gap: 0.9rem;
      padding: 1rem 0;
      border-bottom: 1px solid var(--line);
    }

    .filter-section:last-of-type {
      border-bottom: 0;
      padding-bottom: 0;
    }

    .filter-section h3 {
      margin: 0;
      font-size: 1.05rem;
      font-weight: 600;
    }

    .filter-chip-grid {
      display: flex;
      flex-wrap: wrap;
      gap: 0.75rem;
    }

    .filter-chip {
      display: inline-flex;
      align-items: center;
      gap: 0.35rem;
      border: 1px solid var(--line);
      border-radius: 12px;
      background: transparent;
      color: var(--text);
      padding: 0.65rem 0.85rem;
      cursor: pointer;
    }

    .filter-chip.active {
      border-color: var(--accent);
      color: var(--accent-2);
      background: color-mix(in srgb, var(--accent) 10%, transparent);
    }

    .filter-choice {
      display: flex;
      align-items: center;
      gap: 0.65rem;
      color: var(--text);
      cursor: pointer;
      font-weight: 500;
    }

    .filter-choice input {
      width: auto;
      margin: 0;
    }

    .filter-footer {
      display: flex;
      justify-content: space-between;
      gap: 0.75rem;
      flex-wrap: wrap;
      margin-top: 1rem;
    }

    .icon-button {
      appearance: none;
      border: 0;
      width: 2rem;
      height: 2rem;
      border-radius: 999px;
      background: transparent;
      color: var(--text);
      font-size: 1.2rem;
      cursor: pointer;
    }

    .icon-button:hover {
      background: color-mix(in srgb, var(--accent) 10%, transparent);
    }

    .stack {
      display: grid;
      gap: 0.9rem;
    }

    label {
      display: grid;
      gap: 0.4rem;
      font-weight: 600;
    }

    input {
      width: 100%;
      padding: 0.8rem 0.9rem;
      border: 1px solid var(--line);
      border-radius: 10px;
      background: var(--panel-2);
      color: var(--text);
      outline: none;
    }

    code.inline {
      display: inline-block;
      background: color-mix(in srgb, var(--accent) 12%, transparent);
      border-radius: 6px;
      padding: 0.15rem 0.35rem;
      word-break: break-word;
    }

    .form-actions {
      display: flex;
      gap: 0.75rem;
      flex-wrap: wrap;
      align-items: center;
    }

    .hidden {
      display: none !important;
    }

    .auth-shell,
    .admin-shell {
      display: grid;
      gap: 1rem;
    }

    .auth-card,
    .admin-card {
      border: 1px solid var(--line);
      border-radius: 14px;
      background: var(--panel-2);
      padding: 1rem;
    }

    .auth-card h2,
    .admin-card h2,
    .admin-card h3 {
      margin: 0 0 0.65rem;
    }

    .auth-note {
      color: var(--muted);
      line-height: 1.5;
    }

    .admin-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 1rem;
    }

    .admin-list {
      display: grid;
      gap: 0.75rem;
    }

    .admin-list-item {
      border: 1px solid var(--line);
      border-radius: 12px;
      background: var(--panel-3);
      padding: 0.85rem 0.95rem;
      display: grid;
      gap: 0.75rem;
    }

    .admin-list-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 1rem;
    }

    .admin-inline {
      display: flex;
      flex-wrap: wrap;
      gap: 0.75rem;
      align-items: center;
    }

    .admin-inline > * {
      flex: 1 1 180px;
    }

    .pill {
      display: inline-flex;
      align-items: center;
      border-radius: 999px;
      padding: 0.2rem 0.55rem;
      font-size: 0.78rem;
      font-weight: 700;
      background: color-mix(in srgb, var(--accent) 18%, transparent);
      color: var(--accent-2);
    }

    .sidebar-scrim {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.45);
      opacity: 0;
      pointer-events: none;
      transition: opacity 180ms ease;
      z-index: 20;
    }

    @media (max-width: 980px) {
      .sidebar {
        transform: translateX(-100%);
        width: min(86vw, 340px);
      }

      .main {
        margin-left: 0;
        width: 100vw;
      }

      .content {
        padding: 1rem;
      }

      .toolbar {
        padding: 0.75rem 0.85rem;
      }

      .toolbar-actions {
        gap: 0.45rem;
      }

      .toolbar-button {
        padding: 0.45rem 0.75rem;
      }

      .menu-button {
        display: inline-flex;
      }

      .hero-banner {
        height: 144px;
      }

      .hero-content {
        padding-left: 1rem;
        padding-right: 1rem;
        margin-top: -32px;
      }

      .hero-avatar {
        width: 112px;
        height: 112px;
      }

      .hero-summary {
        font-size: 0.94rem;
        max-width: 100%;
      }

      .tabs {
        width: calc(100% + 2rem);
        padding: 0 1rem;
        gap: 1.1rem;
      }

      .search-row {
        grid-template-columns: 1fr auto;
        gap: 0.65rem;
      }

      .search-input {
        border-right: 1px solid var(--line);
        border-radius: 10px;
      }

      .search-action {
        border-radius: 10px;
      }

      .filter-button {
        grid-column: 1 / -1;
        margin-left: 0;
        border-radius: 10px;
        min-height: 2.8rem;
      }

      .asset-grid,
      .media-grid,
      .info-grid {
        grid-template-columns: 1fr;
      }
    }

    @media (max-width: 720px) {
      .toolbar {
        align-items: stretch;
        flex-direction: column;
      }

      .toolbar-start {
        width: 100%;
      }

      .toolbar-title {
        font-size: 0.82rem;
      }

      .toolbar-actions {
        width: 100%;
        flex-wrap: wrap;
        justify-content: stretch;
      }

      .toolbar-actions > * {
        flex: 1 1 100%;
      }

      .toolbar-button {
        font-size: 0.92rem;
        width: 100%;
      }

      .hero-title {
        font-size: clamp(1.7rem, 8vw, 2.35rem);
      }

      .hero-banner {
        height: 124px;
      }

      .hero-content {
        margin-top: -24px;
      }

      .hero-avatar {
        width: 96px;
        height: 96px;
      }

      .hero-summary {
        font-size: 0.9rem;
        -webkit-line-clamp: 2;
      }

      .tabs {
        gap: 0.85rem;
        font-size: 0.9rem;
      }

      .tab {
        font-size: 0.9rem;
      }

      .content-inner {
        width: 100%;
      }

      .post-body,
      .article,
      .reward-card,
      .info-card {
        padding-left: 0.9rem;
        padding-right: 0.9rem;
      }

      .viewer-stage {
        padding: 4.5rem 1rem 4rem;
      }

      .viewer-nav {
        width: 2.5rem;
        height: 2.5rem;
      }

      .viewer-nav.prev {
        left: 0.5rem;
      }

      .viewer-nav.next {
        right: 0.5rem;
      }
    }

    @media (max-width: 520px) {
      .toolbar {
        padding: 0.65rem 0.7rem;
      }

      .toolbar-start {
        gap: 0.65rem;
      }

      .menu-button {
        width: 2.5rem;
        height: 2.5rem;
      }

      .toolbar-title {
        font-size: 0.76rem;
        line-height: 1.2;
      }

      .toolbar-button {
        padding: 0.55rem 0.7rem;
        font-size: 0.88rem;
      }

      .hero-banner {
        height: 108px;
      }

      .hero-avatar {
        width: 82px;
        height: 82px;
      }

      .hero-title {
        margin-top: 0.75rem;
        font-size: clamp(1.55rem, 8vw, 2rem);
      }

      .hero-summary {
        font-size: 0.86rem;
      }

      .tabs {
        gap: 0.45rem 0.9rem;
      }

      .tab {
        padding-bottom: 0.7rem;
        font-size: 0.86rem;
      }

      .content {
        padding: 0.85rem;
      }

      .search-row {
        gap: 0.5rem;
      }

      .search-action {
        min-width: 3.2rem;
      }
    }

    body.sidebar-open .sidebar {
      transform: translateX(0);
    }

    body.sidebar-open .sidebar-scrim {
      opacity: 1;
      pointer-events: auto;
    }
  </style>
</head>
<body>
  <div class="app">
    <aside class="sidebar">
      <div class="sidebar-section">
        <h2 class="sidebar-heading">Recently downloaded</h2>
        <div id="campaigns" class="creator-list"></div>
      </div>
    </aside>

    <main class="main">
      <div class="toolbar">
        <div class="toolbar-start">
          <button type="button" class="menu-button" id="menu-button" aria-label="Open creators">☰</button>
          <div class="toolbar-title">Creator browser</div>
        </div>
        <div class="toolbar-actions">
          <button type="button" class="toolbar-button hidden" id="admin-button-top">Admin</button>
          <button type="button" class="toolbar-button" id="settings-button-top">Settings</button>
          <button type="button" class="toolbar-button" id="theme-toggle">Switch to Light Mode</button>
          <button type="button" class="toolbar-button hidden" id="logout-button-top">Sign Out</button>
        </div>
      </div>

      <section class="hero" id="hero">
        <div class="hero-banner" id="hero-banner">
          <div class="hero-backdrop"></div>
        </div>
        <div class="hero-content">
          <img id="hero-avatar" class="hero-avatar" alt="" hidden>
          <h1 id="hero-title" class="hero-title">Select a creator</h1>
          <p id="hero-summary" class="hero-summary">Choose a campaign from the sidebar to browse posts, media, and creator details.</p>
          <div class="tabs">
            <button class="tab active" data-tab="posts">Posts</button>
            <button class="tab" data-tab="collections">Collections</button>
            <button class="tab" data-tab="media">Media</button>
            <button class="tab" data-tab="about">About</button>
          </div>
        </div>
      </section>

      <section class="content">
        <div class="content-inner">
          <div id="details"></div>
        </div>
      </section>
    </main>
  </div>

  <div class="sidebar-scrim" id="sidebar-scrim"></div>

  <div class="overlay" id="settings-overlay">
    <div class="modal">
      <div class="modal-header">
        <div>
          <div style="font-weight:700;" id="settings-title">Server settings</div>
          <div style="margin-top:0.35rem;" class="muted" id="settings-description">Update the server port and storage paths.</div>
        </div>
        <button type="button" class="icon-button" id="close-settings" aria-label="Close">×</button>
      </div>
      <div class="modal-body" id="settings-body"></div>
    </div>
  </div>

  <div class="overlay" id="post-filter-overlay">
    <div class="modal">
      <div class="modal-header">
        <div>
          <div style="font-weight:700;">Post filters</div>
          <div style="margin-top:0.35rem;" class="muted">Narrow posts by content type, access, and sort order.</div>
        </div>
        <button type="button" class="icon-button" id="close-post-filter" aria-label="Close">×</button>
      </div>
      <div class="modal-body" id="post-filter-body"></div>
    </div>
  </div>

  <div class="viewer-overlay" id="viewer-overlay">
    <div class="viewer-topbar">
      <div class="viewer-counter" id="viewer-counter">0 / 0</div>
      <div class="viewer-actions">
        <a href="#" class="viewer-button" id="viewer-download" title="Download" aria-label="Download">↓</a>
        <button type="button" class="viewer-button" id="viewer-close" title="Close" aria-label="Close">×</button>
      </div>
    </div>
    <button type="button" class="viewer-nav prev" id="viewer-prev" aria-label="Previous">‹</button>
    <div class="viewer-stage" id="viewer-stage"></div>
    <button type="button" class="viewer-nav next" id="viewer-next" aria-label="Next">›</button>
    <div class="viewer-caption" id="viewer-caption"></div>
  </div>

  <script>
    const rootEl = document.documentElement;
    const campaignsEl = document.getElementById("campaigns");
    const detailsEl = document.getElementById("details");
    const heroEl = document.getElementById("hero");
    const heroBannerEl = document.getElementById("hero-banner");
    const heroAvatarEl = document.getElementById("hero-avatar");
    const heroTitleEl = document.getElementById("hero-title");
    const heroSummaryEl = document.getElementById("hero-summary");
    const tabsEl = [...document.querySelectorAll(".tab")];
    const topSettingsButtonEl = document.getElementById("settings-button-top");
    const adminButtonEl = document.getElementById("admin-button-top");
    const logoutButtonEl = document.getElementById("logout-button-top");
    const menuButtonEl = document.getElementById("menu-button");
    const sidebarScrimEl = document.getElementById("sidebar-scrim");
    const settingsOverlayEl = document.getElementById("settings-overlay");
    const postFilterOverlayEl = document.getElementById("post-filter-overlay");
    const postFilterBodyEl = document.getElementById("post-filter-body");
    const closePostFilterEl = document.getElementById("close-post-filter");
    const settingsBodyEl = document.getElementById("settings-body");
    const settingsTitleEl = document.getElementById("settings-title");
    const settingsDescriptionEl = document.getElementById("settings-description");
    const closeSettingsEl = document.getElementById("close-settings");
    const themeToggleEl = document.getElementById("theme-toggle");
    const viewerOverlayEl = document.getElementById("viewer-overlay");
    const viewerStageEl = document.getElementById("viewer-stage");
    const viewerCounterEl = document.getElementById("viewer-counter");
    const viewerCaptionEl = document.getElementById("viewer-caption");
    const viewerDownloadEl = document.getElementById("viewer-download");
    const viewerCloseEl = document.getElementById("viewer-close");
    const viewerPrevEl = document.getElementById("viewer-prev");
    const viewerNextEl = document.getElementById("viewer-next");

    let runtimeInfo = null;
    let activeCampaign = null;
    let activeCampaignId = null;
    let currentTab = "posts";
    let cachedPosts = [];
    let currentPostPage = 1;
    let currentCollectionPage = 1;
    let viewerItems = [];
    let viewerIndex = 0;
    let viewerTouchStartX = 0;
    let viewerTouchStartY = 0;
    let viewerTouchActive = false;
    let currentView = "browse";
    let browseState = { kind: "campaign-tab", campaignId: null, tab: "posts", page: 1 };
    let adminReturnState = null;
    const inviteToken = new URLSearchParams(window.location.search).get("invite") || "";
    let postFilters = createDefaultPostFilters();
    const postsPerPage = 10;

    initializeTheme();
    wireSettingsOverlay();
    wireTabs();
    wireSidebarMenu();
    wireAuthUi();
    wirePostFilterOverlay();
    wireViewer();

    async function getJson(url) {
      const response = await fetch(url);
      return assertOkJson(response);
    }

    async function postJson(url, body) {
      const response = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
      });

      return assertOkJson(response);
    }

    async function assertOkJson(response) {
      if (response.ok) {
        if (response.status === 204) {
          return {};
        }
        return response.json();
      }

      let errorMessage = `Request failed: ${response.status}`;
      try {
        const json = await response.json();
        errorMessage = json?.error || json?.detail || json?.title || errorMessage;
      } catch (_) {
        const text = await response.text();
        if (text) {
          errorMessage = text;
        }
      }

      throw new Error(errorMessage);
    }

    async function loadRuntime() {
      runtimeInfo = await getJson("/api/server/runtime");
      document.title = runtimeInfo?.config?.pageTitle || "Patreon Browse Server";
      updateToolbarState();

      if (runtimeInfo.setupRequired) {
        topSettingsButtonEl.disabled = true;
        adminButtonEl.classList.add("hidden");
        logoutButtonEl.classList.add("hidden");
        renderSetup(
          "First-run setup",
          "Set the port, the Patreon data directory, and where this server should store writable files.",
          true
        );
        campaignsEl.innerHTML = "";
        renderHero(null);
        setHeroVisible(false);
        renderWaitingState("Complete server setup to begin browsing downloaded content.");
        openSettings();
        return;
      }

      if (runtimeInfo.auth?.requiresBootstrap) {
        closeSettings();
        renderHero(null);
        setHeroVisible(false);
        campaignsEl.innerHTML = "";
        renderBootstrapAccount();
        return;
      }

      if (!runtimeInfo.auth?.authenticated) {
        closeSettings();
        renderHero(null);
        setHeroVisible(false);
        campaignsEl.innerHTML = "";
        if (inviteToken) {
          await renderInviteRegistration(inviteToken);
        } else {
          renderLogin();
        }
        return;
      }

      topSettingsButtonEl.disabled = !runtimeInfo.auth?.currentUser?.isAdmin;
      closeSettings();
      setHeroVisible(true);
      await loadCampaigns();
    }

    function renderHero(campaign) {
      if (!campaign) {
        heroEl.style.backgroundImage = "";
        heroBannerEl.querySelector(".hero-cover")?.remove();
        heroAvatarEl.hidden = true;
        heroAvatarEl.removeAttribute("src");
        heroTitleEl.textContent = "Select a creator";
        heroSummaryEl.textContent = "Choose a campaign from the sidebar to browse posts, media, and creator details.";
        return;
      }

      const existingCover = heroBannerEl.querySelector(".hero-cover");
      if (existingCover) {
        existingCover.remove();
      }

      const coverSrc = getCampaignCoverSrc(campaign);
      if (coverSrc) {
        const cover = document.createElement("img");
        cover.className = "hero-cover";
        cover.alt = "";
        cover.src = coverSrc;
        heroBannerEl.insertBefore(cover, heroBannerEl.firstChild);
      }

      const avatarSrc = getCampaignAvatarSrc(campaign);
      if (avatarSrc) {
        heroAvatarEl.hidden = false;
        heroAvatarEl.src = avatarSrc;
        heroAvatarEl.alt = campaign.name || "Creator avatar";
      } else {
        heroAvatarEl.hidden = true;
        heroAvatarEl.removeAttribute("src");
      }

      heroTitleEl.textContent = campaign.name || "Untitled creator";
      heroSummaryEl.textContent = getCampaignSummary(campaign);
    }

    function setHeroVisible(visible) {
      heroEl.classList.toggle("hidden", !visible);
    }

    function updateToolbarState() {
      const isAdmin = !!runtimeInfo?.auth?.currentUser?.isAdmin;
      const isAuthenticated = !!runtimeInfo?.auth?.authenticated;
      adminButtonEl.classList.toggle("hidden", !isAdmin);
      logoutButtonEl.classList.toggle("hidden", !isAuthenticated);
      topSettingsButtonEl.classList.toggle("hidden", !isAdmin);
      topSettingsButtonEl.disabled = !isAdmin;
    }

    function renderWaitingState(text) {
      detailsEl.innerHTML = `
        <div class="empty-state">
          <strong>Waiting for configuration</strong>
          <div class="muted" style="margin-top:0.5rem;">${escapeHtml(text)}</div>
        </div>
      `;
    }

    function renderLogin(message = "") {
      currentView = "auth";
      detailsEl.innerHTML = `
        <div class="content-inner auth-shell">
          <div class="auth-card">
            <h2>Sign in</h2>
            <p class="auth-note">All site content requires an account. Sign in to browse downloaded creators, posts, and media.</p>
            <form id="login-form" class="stack" style="margin-top:1rem;">
              <label>
                Username
                <input type="text" name="userName" autocomplete="username" required>
              </label>
              <label>
                Password
                <input type="password" name="password" autocomplete="current-password" required>
              </label>
              <div class="form-actions">
                <button type="submit" class="primary-button">Sign In</button>
              </div>
              <div id="login-message">${message ? `<div class="message error">${escapeHtml(message)}</div>` : ""}</div>
            </form>
          </div>
        </div>
      `;

      document.getElementById("login-form").onsubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        try {
          await postJson("/api/auth/login", {
            userName: String(formData.get("userName") || ""),
            password: String(formData.get("password") || "")
          });
          await loadRuntime();
        } catch (error) {
          renderLogin(error.message);
        }
      };
    }

    function renderBootstrapAccount(message = "") {
      currentView = "auth";
      detailsEl.innerHTML = `
        <div class="content-inner auth-shell">
          <div class="auth-card">
            <h2>Create the first admin account</h2>
            <p class="auth-note">This first account becomes the site administrator and can later invite or create other users.</p>
            <form id="bootstrap-form" class="stack" style="margin-top:1rem;">
              <label>
                Username
                <input type="text" name="userName" autocomplete="username" required>
              </label>
              <label>
                Password
                <input type="password" name="password" autocomplete="new-password" minlength="8" pattern="\\S+" title="Password cannot contain spaces." required>
              </label>
              <div class="form-actions">
                <button type="submit" class="primary-button">Create Admin Account</button>
              </div>
              <div id="bootstrap-message">${message ? `<div class="message error">${escapeHtml(message)}</div>` : ""}</div>
            </form>
          </div>
        </div>
      `;

      document.getElementById("bootstrap-form").onsubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        try {
          await postJson("/api/auth/bootstrap", {
            userName: String(formData.get("userName") || ""),
            password: String(formData.get("password") || "")
          });
          await loadRuntime();
        } catch (error) {
          renderBootstrapAccount(error.message);
        }
      };
    }

    async function renderInviteRegistration(token, message = "") {
      currentView = "auth";
      let invite;
      try {
        invite = await getJson(`/api/auth/invites/${encodeURIComponent(token)}`);
      } catch (error) {
        detailsEl.innerHTML = `
          <div class="content-inner auth-shell">
            <div class="auth-card">
              <h2>Invite link invalid</h2>
              <p class="auth-note">${escapeHtml(error.message)}</p>
            </div>
          </div>
        `;
        return;
      }

      detailsEl.innerHTML = `
        <div class="content-inner auth-shell">
          <div class="auth-card">
            <h2>Create your account</h2>
            <p class="auth-note">You were invited to join this server. Complete your account below to start browsing.</p>
            <form id="invite-register-form" class="stack" style="margin-top:1rem;">
              <label>
                Username
                <input type="text" name="userName" autocomplete="username" required>
              </label>
              <label>
                Password
                <input type="password" name="password" autocomplete="new-password" minlength="8" pattern="\\S+" title="Password cannot contain spaces." required>
              </label>
              <div class="form-actions">
                <button type="submit" class="primary-button">Create Account</button>
              </div>
              <div class="muted">Invite expires: ${escapeHtml(invite.expiresAt || "No expiry")}</div>
              <div id="invite-register-message">${message ? `<div class="message error">${escapeHtml(message)}</div>` : ""}</div>
            </form>
          </div>
        </div>
      `;

      document.getElementById("invite-register-form").onsubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        try {
          await postJson("/api/auth/register", {
            token,
            userName: String(formData.get("userName") || ""),
            password: String(formData.get("password") || "")
          });
          await loadRuntime();
        } catch (error) {
          await renderInviteRegistration(token, error.message);
        }
      };
    }

    function renderSetup(title, description, forced) {
      const config = runtimeInfo.config;
      const needsBootstrapAccount = forced && !runtimeInfo?.auth?.authenticated;
      settingsTitleEl.textContent = title;
      settingsDescriptionEl.textContent = description;
      settingsBodyEl.innerHTML = `
        <form id="setup-form" class="stack">
          <div class="message">
            <strong>Config file</strong>
            <div style="margin-top:0.4rem;"><code class="inline">${escapeHtml(runtimeInfo.configPath)}</code></div>
          </div>

          <label>
            Port
            <input type="number" min="1024" max="65535" name="port" value="${escapeAttr(config.port || runtimeInfo.currentPort || 5000)}" required>
          </label>

          <label>
            Patreon data directory
            <input type="text" name="dataDirectory" value="${escapeAttr(config.dataDirectory || "")}" placeholder="/path/to/download/root" required>
          </label>

          <label>
            Server storage directory
            <input type="text" name="storageDirectory" value="${escapeAttr(config.storageDirectory || "")}" required>
          </label>

          <label>
            Public base URL
            <input type="url" name="publicBaseUrl" value="${escapeAttr(config.publicBaseUrl || "")}" placeholder="https://example.com">
          </label>

          <label>
            Browser tab title
            <input type="text" name="pageTitle" value="${escapeAttr(config.pageTitle || "Patreon Browse Server")}" placeholder="Patreon Browse Server" required>
          </label>

          ${needsBootstrapAccount ? `
            <div class="message">
              <strong>First admin account</strong>
              <div class="muted" style="margin-top:0.4rem;">Create the first administrator account now so the server can lock down access immediately after setup.</div>
            </div>

            <label>
              Admin username
              <input type="text" name="adminUserName" autocomplete="username" required>
            </label>

            <label>
              Admin password
              <input type="password" name="adminPassword" autocomplete="new-password" minlength="8" pattern="\\S+" title="Password cannot contain spaces." required>
            </label>
          ` : ""}

          <div class="form-actions">
            <button type="submit" class="primary-button">${needsBootstrapAccount ? "Save Configuration and Create Admin" : "Save Configuration"}</button>
            ${forced ? "" : '<button type="button" class="secondary-button" id="cancel-settings">Cancel</button>'}
          </div>

          <div id="setup-message"></div>
        </form>
      `;

      const form = document.getElementById("setup-form");
      form.onsubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData(form);
        const payload = {
          port: Number(formData.get("port")),
          dataDirectory: String(formData.get("dataDirectory") || ""),
          storageDirectory: String(formData.get("storageDirectory") || ""),
          publicBaseUrl: String(formData.get("publicBaseUrl") || ""),
          pageTitle: String(formData.get("pageTitle") || "")
        };

        try {
          const result = await postJson("/api/server/setup", payload);
          if (needsBootstrapAccount) {
            await postJson("/api/auth/bootstrap", {
              userName: String(formData.get("adminUserName") || ""),
              password: String(formData.get("adminPassword") || "")
            });
          }

          document.getElementById("setup-message").innerHTML = `
            <div class="message success">
              <strong>Saved</strong>
              <div class="muted" style="margin-top:0.4rem;">
                ${needsBootstrapAccount
                  ? "Configuration saved and the first admin account was created."
                  : result.restartRequired
                    ? "Configuration saved. Restart the server to apply the saved port."
                    : "Configuration saved."}
              </div>
            </div>
          `;

          await loadRuntime();
          if (!forced) {
            closeSettings();
          }
        } catch (error) {
          document.getElementById("setup-message").innerHTML = `
            <div class="message error">
              <strong>Save failed</strong>
              <div class="muted" style="margin-top:0.4rem;">${escapeHtml(error.message)}</div>
            </div>
          `;
        }
      };

      const cancelButton = document.getElementById("cancel-settings");
      if (cancelButton) {
        cancelButton.onclick = () => closeSettings();
      }
    }

    async function loadCampaigns() {
      campaignsEl.innerHTML = `
        <div class="creator-status">
          Loading creators from <code class="inline">${escapeHtml(runtimeInfo?.config?.dataDirectory || "the configured data directory")}</code>...
        </div>
      `;

      let data;
      try {
        data = await getJson("/api/campaigns?n=100");
      } catch (error) {
        campaignsEl.innerHTML = `
          <div class="message error">
            <strong>Creators failed to load</strong>
            <div class="muted" style="margin-top:0.5rem;">${escapeHtml(error.message)}</div>
            <div class="muted" style="margin-top:0.5rem;">Check the Patreon data directory in Settings and make sure <code class="inline">${escapeHtml((runtimeInfo?.config?.dataDirectory || "") + "/.patreon-dl/db.sqlite")}</code> exists.</div>
          </div>
        `;
        renderHero(null);
        detailsEl.innerHTML = `
          <div class="message error">
            <strong>Unable to browse campaigns</strong>
            <div class="muted" style="margin-top:0.5rem;">The server could not read the Patreon database from the configured data directory.</div>
          </div>
        `;
        return;
      }

      campaignsEl.innerHTML = `
        <div class="creator-status">
          Loaded ${escapeHtml(String(data.total || data.campaigns?.length || 0))} creator${Number(data.total || data.campaigns?.length || 0) === 1 ? "" : "s"}
        </div>
      `;

      if (!data.campaigns || data.campaigns.length === 0) {
        campaignsEl.innerHTML += `
          <div class="empty-state">
            <strong>No creators found</strong>
            <div class="muted" style="margin-top:0.5rem;">No downloaded campaigns were found in the configured Patreon data directory.</div>
          </div>
        `;
        renderHero(null);
        detailsEl.innerHTML = `
          <div class="empty-state">
            <strong>No campaigns found</strong>
            <div class="muted" style="margin-top:0.5rem;">Check the configured Patreon data directory from Settings.</div>
          </div>
        `;
        return;
      }

      for (const campaign of data.campaigns) {
        const button = document.createElement("button");
        button.className = "creator-item" + (campaign.id === activeCampaignId ? " active" : "");
        const avatarSrc = getCampaignAvatarSrc(campaign);
        const name = campaign.name || campaign.campaign_name || campaign.id || "Creator";
        const avatarHtml = avatarSrc
          ? `<img class="creator-avatar" src="${escapeAttr(avatarSrc)}" alt="">`
          : `<div class="creator-avatar placeholder">${escapeHtml(getInitials(name))}</div>`;
        button.innerHTML = `
          ${avatarHtml}
          <span class="creator-name">${escapeHtml(name)}</span>
        `;
        button.onclick = () => loadCampaign(campaign.id);
        campaignsEl.appendChild(button);
      }

      if (!activeCampaignId) {
        await loadCampaign(data.campaigns[0].id);
      }
    }

    async function loadCampaign(id) {
      document.body.classList.remove("sidebar-open");
      currentView = "browse";
      setHeroVisible(true);
      activeCampaignId = id;
      currentTab = "posts";
      cachedPosts = [];
      currentPostPage = 1;
      browseState = { kind: "campaign-tab", campaignId: id, tab: "posts", page: 1 };
      updateTabs();

      const campaign = await getJson(`/api/campaigns/${id}?with_counts=true`);
      activeCampaign = campaign;
      renderHero(campaign);
      await loadCampaigns();
      await renderCurrentTab();
    }

    async function renderCurrentTab() {
      if (!activeCampaignId || !activeCampaign) {
        return;
      }

      switch (currentTab) {
        case "collections":
          await renderCollectionsTab(activeCampaignId, activeCampaign);
          break;
        case "media":
          await renderMediaTab(activeCampaignId, activeCampaign);
          break;
        case "about":
          renderAboutTab(activeCampaign);
          break;
        case "posts":
        default:
          await renderPostsTab(activeCampaignId, activeCampaign);
          break;
      }
    }

    async function renderPostsTab(id, campaign) {
      if (!cachedPosts.length) {
        detailsEl.innerHTML = `
          <div class="message">
            <strong>Loading posts</strong>
            <div class="muted" style="margin-top:0.5rem;">Gathering all posts for this creator so search, filters, and pagination work across the full library.</div>
          </div>
        `;

        cachedPosts = await loadAllCampaignPosts(id);
      }

      renderPostsList(id, cachedPosts.length);
    }

    function renderPostsList(id, totalPosts) {
      const filteredPosts = getFilteredPosts(cachedPosts);
      const pageCount = Math.max(1, Math.ceil(filteredPosts.length / postsPerPage));
      currentPostPage = Math.min(Math.max(1, currentPostPage), pageCount);
      browseState = { kind: "campaign-tab", campaignId: id, tab: "posts", page: currentPostPage };
      const pageStart = (currentPostPage - 1) * postsPerPage;
      const pagedPosts = filteredPosts.slice(pageStart, pageStart + postsPerPage);
      detailsEl.innerHTML = `
        <div class="search-row">
          <input class="search-input" type="text" placeholder="Search posts" id="search-posts" value="${escapeAttr(postFilters.search)}">
          <button class="search-action" type="button" title="Search">
            <svg viewBox="0 0 24 24" width="24" height="24" fill="currentColor" aria-hidden="true"><path d="M10 4a6 6 0 1 0 3.874 10.582l4.272 4.272 1.414-1.414-4.272-4.272A6 6 0 0 0 10 4Zm0 2a4 4 0 1 1 0 8a4 4 0 0 1 0-8Z"/></svg>
          </button>
          <button class="filter-button" type="button" id="post-filter-button">Filters</button>
        </div>
        <div class="counter">${filteredPosts.length === totalPosts ? `Total ${totalPosts} posts` : `Showing ${filteredPosts.length} of ${totalPosts} posts`}</div>
      `;

      const list = document.createElement("div");
      list.className = "post-list";

      for (const post of pagedPosts) {
        const card = document.createElement("article");
        card.className = "post-card";
        const previewItems = collectPostPreviewItems(post);
        card.innerHTML = `
          ${renderPostPreviewMedia(previewItems)}
          <div class="post-body">
            <h3 class="post-title"><a href="#" class="post-title-link">${escapeHtml(post.title || post.name || post.id)}</a></h3>
            <div class="post-meta">${escapeHtml(post.publishedAt || "No publish date")}</div>
          </div>
        `;
        card.querySelector(".post-title-link").onclick = async (event) => {
          event.preventDefault();
          await loadPost(post.id, id);
        };
        for (const tile of card.querySelectorAll("[data-preview-index]")) {
          const index = Number(tile.getAttribute("data-preview-index"));
          tile.addEventListener("click", () => openViewer(previewItems, index));
        }
        list.appendChild(card);
      }

      detailsEl.appendChild(list);
      detailsEl.appendChild(renderPostPagination(id, filteredPosts.length));
      wirePostSearch(id, totalPosts);
      document.getElementById("post-filter-button").onclick = () => openPostFilterOverlay(id, totalPosts);
    }

    async function loadAllCampaignPosts(campaignId) {
      const pageSize = 100;
      let page = 1;
      let total = 0;
      const items = [];

      do {
        const response = await getJson(`/api/campaigns/${campaignId}/posts?sort_by=latest&n=${pageSize}&p=${page}`);
        const batch = response.items || [];
        total = Number(response.total || 0);
        items.push(...batch);
        if (!batch.length) {
          break;
        }
        page += 1;
      } while (items.length < total);

      return items;
    }

    function renderPostPagination(campaignId, filteredCount) {
      const pageCount = Math.max(1, Math.ceil(filteredCount / postsPerPage));
      const wrapper = document.createElement("div");
      wrapper.className = "pagination";

      const start = filteredCount ? ((currentPostPage - 1) * postsPerPage) + 1 : 0;
      const end = Math.min(currentPostPage * postsPerPage, filteredCount);

      wrapper.innerHTML = `
        <div>${filteredCount ? `Showing ${start}-${end} of ${filteredCount}` : "Showing 0 posts"}</div>
        <div class="pagination-controls">
          <button type="button" class="page-button" id="posts-page-prev" ${currentPostPage <= 1 ? "disabled" : ""}>Previous</button>
          <div>Page ${currentPostPage} of ${pageCount}</div>
          <button type="button" class="page-button" id="posts-page-next" ${currentPostPage >= pageCount ? "disabled" : ""}>Next</button>
        </div>
      `;

      wrapper.querySelector("#posts-page-prev").onclick = () => {
        if (currentPostPage <= 1) {
          return;
        }
        currentPostPage -= 1;
        renderPostsList(campaignId, cachedPosts.length);
      };

      wrapper.querySelector("#posts-page-next").onclick = () => {
        if (currentPostPage >= pageCount) {
          return;
        }
        currentPostPage += 1;
        renderPostsList(campaignId, cachedPosts.length);
      };

      return wrapper;
    }

    async function renderMediaTab(id, campaign) {
      browseState = { kind: "campaign-tab", campaignId: id, tab: "media", page: 1 };
      if (!cachedPosts.length) {
        const posts = await getJson(`/api/campaigns/${id}/posts?sort_by=latest&n=100`);
        cachedPosts = posts.items || [];
      }

      const mediaItems = extractMediaItems(cachedPosts);
      detailsEl.innerHTML = `
        <div class="counter">Total ${mediaItems.length} media items collected from downloaded posts</div>
      `;

      if (!mediaItems.length) {
        detailsEl.innerHTML = `
          <div class="empty-state">
            <strong>No media previews available</strong>
            <div class="muted" style="margin-top:0.5rem;">This creator does not have locally stored media in the current dataset.</div>
          </div>
        `;
        return;
      }

      const list = document.createElement("div");
      list.className = "media-grid";

      for (const item of mediaItems) {
        const card = document.createElement("article");
        card.className = "media-card";
        const index = mediaItems.indexOf(item);
        card.innerHTML = `
          <img class="media-thumb" src="${escapeAttr(item.previewSrc || item.src)}" alt="">
          <div class="media-body">
            <h3 class="media-title"><a href="#" class="post-title-link media-post-link">${escapeHtml(item.title)}</a></h3>
            <div class="media-subtitle">${escapeHtml(item.kind)} • ${escapeHtml(item.publishedAt)}</div>
          </div>
        `;
        card.querySelector(".media-thumb").onclick = () => openViewer(mediaItems, index);
        card.querySelector(".media-post-link").onclick = async (event) => {
          event.preventDefault();
          await loadPost(item.postId, id);
        };
        list.appendChild(card);
      }

      detailsEl.appendChild(list);
    }

    async function renderCollectionsTab(id, campaign) {
      browseState = { kind: "campaign-tab", campaignId: id, tab: "collections", page: 1 };
      const data = await getJson(`/api/campaigns/${id}/collections?sort_by=last_updated&n=100`);
      const collections = data.collections || [];

      detailsEl.innerHTML = `
        <div class="counter">Total ${data.total || collections.length || 0} collections</div>
      `;

      if (!collections.length) {
        detailsEl.innerHTML = `
          <div class="empty-state">
            <strong>No collections available</strong>
            <div class="muted" style="margin-top:0.5rem;">This creator does not have any collections stored in the current dataset.</div>
          </div>
        `;
        return;
      }

      const list = document.createElement("div");
      list.className = "post-list";

      for (const collection of collections) {
        const card = document.createElement("article");
        card.className = "post-card";
        const title = collection.title || collection.name || collection.id || "Untitled collection";
        const description = sanitizeText(collection.description || collection.summary || "No description available.");
        const postCount = collection.numPosts || collection.postCount || 0;
        const createdAt = collection.createdAt || collection.editedAt || collection.lastUpdatedAt || "No timestamp";
        const iconSrc = getCollectionIconSrc(collection);
        const iconHtml = iconSrc
          ? `<img class="collection-icon" src="${escapeAttr(iconSrc)}" alt="">`
          : `<div class="collection-icon placeholder">${escapeHtml(getInitials(title))}</div>`;

        card.innerHTML = `
          <div class="post-body">
            <div class="collection-header">
              ${iconHtml}
              <div>
                <h3 class="post-title"><a href="#" class="collection-link">${escapeHtml(title)}</a></h3>
                <div class="post-meta">${escapeHtml(createdAt)} • ${escapeHtml(String(postCount))} posts</div>
              </div>
            </div>
            <div class="reward-description">${escapeHtml(description)}</div>
          </div>
        `;
        card.querySelector(".collection-link").onclick = async (event) => {
          event.preventDefault();
          await loadCollection(collection.id, id);
        };
        list.appendChild(card);
      }

      detailsEl.appendChild(list);
    }

    async function loadCollection(collectionId, campaignId, page = 1) {
      currentCollectionPage = Math.max(1, page);
      browseState = { kind: "collection-detail", campaignId, collectionId, page: currentCollectionPage };
      const data = await getJson(`/api/collections/${collectionId}`);
      const collection = data.collection || {};
      const posts = await getJson(`/api/collections/${collectionId}/posts?sort_by=latest&n=${postsPerPage}&p=${currentCollectionPage}`);
      const title = collection.title || collection.name || collection.id || "Untitled collection";
      const description = sanitizeText(collection.description || collection.summary || "");
      const iconSrc = getCollectionIconSrc(collection);
      const iconHtml = iconSrc
        ? `<img class="collection-icon" src="${escapeAttr(iconSrc)}" alt="">`
        : `<div class="collection-icon placeholder">${escapeHtml(getInitials(title))}</div>`;

      detailsEl.innerHTML = `
        <div class="article">
          <div style="margin-bottom:1rem;">
            <a href="#" class="action-link" id="back-to-collections">← Back to collections</a>
          </div>
          <div class="collection-header">
            ${iconHtml}
            <div>
              <h2>${escapeHtml(title)}</h2>
              <div class="article-meta">${escapeHtml(String(collection.numPosts || posts.total || 0))} posts</div>
            </div>
          </div>
          ${description ? `<div class="reward-description" style="margin-bottom:1rem;">${escapeHtml(description)}</div>` : ""}
          <div class="counter">Total ${posts.total || 0} posts in this collection</div>
        </div>
      `;

      const list = document.createElement("div");
      list.className = "post-list";

      for (const post of posts.items || []) {
        const card = document.createElement("article");
        card.className = "post-card";
        const previewItems = collectPostPreviewItems(post);
        card.innerHTML = `
          ${renderPostPreviewMedia(previewItems)}
          <div class="post-body">
            <h3 class="post-title"><a href="#" class="post-title-link">${escapeHtml(post.title || post.name || post.id)}</a></h3>
            <div class="post-meta">${escapeHtml(post.publishedAt || "No publish date")}</div>
          </div>
        `;
        card.querySelector(".post-title-link").onclick = async (event) => {
          event.preventDefault();
          await loadPost(post.id, campaignId);
        };
        for (const tile of card.querySelectorAll("[data-preview-index]")) {
          const index = Number(tile.getAttribute("data-preview-index"));
          tile.addEventListener("click", () => openViewer(previewItems, index));
        }
        list.appendChild(card);
      }

      detailsEl.appendChild(list);
      detailsEl.appendChild(renderCollectionPagination(collectionId, campaignId, posts.total || 0));

      document.getElementById("back-to-collections").onclick = async (event) => {
        event.preventDefault();
        currentTab = "collections";
        updateTabs();
        await renderCollectionsTab(campaignId, activeCampaign);
      };
    }

    function renderCollectionPagination(collectionId, campaignId, totalPosts) {
      const pageCount = Math.max(1, Math.ceil(totalPosts / postsPerPage));
      currentCollectionPage = Math.min(Math.max(1, currentCollectionPage), pageCount);
      const start = totalPosts ? ((currentCollectionPage - 1) * postsPerPage) + 1 : 0;
      const end = Math.min(currentCollectionPage * postsPerPage, totalPosts);

      const wrapper = document.createElement("div");
      wrapper.className = "pagination";
      wrapper.innerHTML = `
        <div>${totalPosts ? `Showing ${start}-${end} of ${totalPosts}` : "Showing 0 posts"}</div>
        <div class="pagination-controls">
          <button type="button" class="page-button" id="collection-page-prev" ${currentCollectionPage <= 1 ? "disabled" : ""}>Previous</button>
          <div>Page ${currentCollectionPage} of ${pageCount}</div>
          <button type="button" class="page-button" id="collection-page-next" ${currentCollectionPage >= pageCount ? "disabled" : ""}>Next</button>
        </div>
      `;

      wrapper.querySelector("#collection-page-prev").onclick = async () => {
        if (currentCollectionPage <= 1) {
          return;
        }
        await loadCollection(collectionId, campaignId, currentCollectionPage - 1);
      };

      wrapper.querySelector("#collection-page-next").onclick = async () => {
        if (currentCollectionPage >= pageCount) {
          return;
        }
        await loadCollection(collectionId, campaignId, currentCollectionPage + 1);
      };

      return wrapper;
    }

    function renderAboutTab(campaign) {
      browseState = { kind: "campaign-tab", campaignId: activeCampaignId, tab: "about", page: 1 };
      const rewards = Array.isArray(campaign.rewards) ? campaign.rewards : [];
      const creatorName = campaign?.creator?.fullName || campaign?.creator?.full_name || "Unknown creator";
      const vanity = campaign?.creator?.vanity || campaign?.creator?.id || "Unavailable";
      const summary = getCampaignSummary(campaign);

      detailsEl.innerHTML = `
        <div class="article">
          <h2>${escapeHtml(campaign.name || "Creator")}</h2>
          <div class="article-meta">${escapeHtml(summary)}</div>
          <div class="info-grid">
            <div class="info-card">
              <div class="info-label">Creator</div>
              <div class="info-value">${escapeHtml(creatorName)}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Vanity / Handle</div>
              <div class="info-value">${escapeHtml(vanity)}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Campaign URL</div>
              <div class="info-value">${campaign.url ? `<a class="action-link" href="${escapeAttr(campaign.url)}" target="_blank" rel="noreferrer">${escapeHtml(campaign.url)}</a>` : "Unavailable"}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Currency</div>
              <div class="info-value">${escapeHtml(campaign.currency || "Unavailable")}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Posts</div>
              <div class="info-value">${campaign.postCount || 0}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Media</div>
              <div class="info-value">${campaign.mediaCount || 0}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Products</div>
              <div class="info-value">${campaign.productCount || 0}</div>
            </div>
            <div class="info-card">
              <div class="info-label">Collections</div>
              <div class="info-value">${campaign.collectionCount || 0}</div>
            </div>
          </div>
          <div class="reward-list" id="reward-list"></div>
        </div>
      `;

      const rewardListEl = document.getElementById("reward-list");
      if (!rewards.length) {
        rewardListEl.innerHTML = `
          <div class="empty-state">
            <strong>No reward tiers in this dataset</strong>
            <div class="muted" style="margin-top:0.5rem;">This campaign does not have reward tier information stored in the downloaded database.</div>
          </div>
        `;
        return;
      }

      rewardListEl.insertAdjacentHTML("beforebegin", `<h3 style="margin:1.25rem 0 0.75rem;">Reward tiers</h3>`);

      for (const reward of rewards) {
        const card = document.createElement("article");
        card.className = "reward-card";
        const description = sanitizeText(reward.description || reward.title || "No description available.");
        card.innerHTML = `
          <h4 class="reward-title">${escapeHtml(reward.title || "Untitled reward")}</h4>
          <div class="reward-description">${escapeHtml(description)}</div>
        `;
        rewardListEl.appendChild(card);
      }
    }

    async function loadPost(postId, campaignId) {
      browseState = { kind: "post-detail", campaignId, postId, page: currentPostPage };
      const data = await getJson(`/api/posts/${postId}`);
      const post = data.post;
      const bodyHtml = post.content || "";
      const description = sanitizeText(post.description || post.excerpt || post.teaserText || post.teaser_text || "");
      const imageAssets = collectPostImageAssets(post);
      const mediaAssets = collectPostRichMediaAssets(post);
      const viewerMediaAssets = mediaAssets.filter((item) => item.type === "video");
      const attachments = collectPostAttachments(post);
      detailsEl.innerHTML = `
        <div class="article">
          <div style="margin-bottom:1rem;">
            <a href="#" class="action-link" id="back-button">← Back to posts</a>
          </div>
          <h2>${escapeHtml(post.title || "Untitled post")}</h2>
          <div class="article-meta">${escapeHtml(post.publishedAt || "No publish date")} • ${post.commentCount || 0} comments</div>
          ${description ? `<div class="reward-description" style="margin-bottom:1rem;">${escapeHtml(description)}</div>` : ""}
          <div class="content-body">${bodyHtml || "<p class='muted'>No post body stored in the database.</p>"}</div>
          ${renderPostAssetSection("Images", imageAssets, "images")}
          ${renderPostAssetSection("Video and audio", mediaAssets, "media")}
          ${renderPostAttachmentSection(attachments)}
        </div>
      `;

      bindViewerSection("images", imageAssets);
      bindViewerSection("media", viewerMediaAssets);

      document.getElementById("back-button").onclick = async (event) => {
        event.preventDefault();
        await renderPostsTab(campaignId, activeCampaign);
      };
    }

    function wirePostSearch(campaignId, totalPosts) {
      const input = document.getElementById("search-posts");
      if (!input) {
        return;
      }

      input.addEventListener("input", () => {
        postFilters.search = input.value;
        currentPostPage = 1;
        renderPostsList(campaignId, totalPosts);
      });
    }

    function extractMediaItems(posts) {
      const items = [];

      for (const post of posts) {
        const title = post.title || post.name || post.id || "Untitled post";
        const publishedAt = post.publishedAt || "No publish date";
        const postId = post.id;

        const candidates = [];

        if (Array.isArray(post.images)) {
          for (const image of post.images) {
            if (image?.id) {
              const fileName = getMediaAssetName(image) || "Image";
              candidates.push({
                kind: "Image",
                src: buildMediaUrl(image.id),
                previewSrc: buildMediaUrl(image.id),
                downloadSrc: buildMediaUrl(image.id, { download: true, name: fileName }),
                title,
                label: fileName,
                type: "image"
              });
            }
          }
        }

        if (Array.isArray(post.attachments)) {
          for (const attachment of post.attachments) {
            if (attachment?.id && isDisplayableImageAttachment(attachment)) {
              const fileName = getMediaAssetName(attachment) || "Image attachment";
              candidates.push({
                kind: "Attachment",
                src: buildMediaUrl(attachment.id),
                previewSrc: buildMediaUrl(attachment.id),
                downloadSrc: buildMediaUrl(attachment.id, { download: true, name: fileName }),
                title,
                label: fileName,
                type: "image"
              });
            }
          }
        }

        const singletons = [
          { value: post.coverImage, kind: "Cover image", type: "image", previewSrc: (value) => buildMediaUrl(value.id) },
          { value: post.thumbnail, kind: "Thumbnail", type: "image", previewSrc: (value) => buildMediaUrl(value.id) },
          { value: post.videoPreview, kind: "Video preview", type: "video", previewSrc: (value) => buildMediaUrl(value.id) },
          { value: post.audioPreview, kind: "Audio preview", type: "audio", previewSrc: (value) => buildMediaUrl(value.id) },
          { value: post.video, kind: "Video", type: "video", previewSrc: (value) => post.videoPreview?.id ? buildMediaUrl(post.videoPreview.id) : buildMediaUrl(value.id) },
          { value: post.audio, kind: "Audio", type: "audio", previewSrc: (value) => post.audioPreview?.id ? buildMediaUrl(post.audioPreview.id) : buildMediaUrl(value.id) }
        ];

        for (const entry of singletons) {
          if (entry.value?.id) {
            const fileName = getMediaAssetName(entry.value) || entry.kind;
            candidates.push({
              kind: entry.kind,
              src: buildMediaUrl(entry.value.id),
              previewSrc: entry.previewSrc(entry.value),
              downloadSrc: buildMediaUrl(entry.value.id, { download: true, name: fileName }),
              title,
              label: fileName,
              type: entry.type
            });
          }
        }

        for (const candidate of candidates) {
          items.push({
            postId,
            publishedAt,
            ...candidate
          });
        }
      }

      const seen = new Set();
      return items.filter((item) => {
        const key = `${item.postId}:${item.src}`;
        if (seen.has(key)) {
          return false;
        }
        seen.add(key);
        return true;
      });
    }

    function collectPostPreviewItems(post) {
      const items = [];
      const seen = new Set();

      const pushItem = (item) => {
        if (!item?.id || seen.has(item.id)) {
          return;
        }

        seen.add(item.id);
        items.push(item);
      };

      for (const image of collectPostImageAssets(post)) {
        pushItem(image);
      }

      for (const media of collectPostRichMediaAssets(post)) {
        if (media.type === "video") {
          pushItem(media);
        }
      }

      return items;
    }

    function createDefaultPostFilters(search = "") {
      return {
        search,
        access: "all",
        sortBy: "latest",
        contentTypes: []
      };
    }

    function clonePostFilters(filters) {
      return {
        search: String(filters?.search || ""),
        access: filters?.access === "accessible" ? "accessible" : "all",
        sortBy: ["latest", "oldest", "a-z", "z-a"].includes(filters?.sortBy) ? filters.sortBy : "latest",
        contentTypes: Array.isArray(filters?.contentTypes) ? [...filters.contentTypes] : []
      };
    }

    function getFilteredPosts(posts) {
      const search = String(postFilters.search || "").trim().toLowerCase();
      const filtered = posts.filter((post) => {
        const title = String(post.title || post.name || post.id || "").toLowerCase();
        if (search && !title.includes(search)) {
          return false;
        }

        if (postFilters.access === "accessible") {
          const isViewable = post.isViewable ?? post.is_viewable;
          if (!isViewable) {
            return false;
          }
        }

        if (postFilters.contentTypes.length) {
          const types = getPostContentTypes(post);
          if (!postFilters.contentTypes.some((type) => types.has(type))) {
            return false;
          }
        }

        return true;
      });

      const sorted = [...filtered];
      sorted.sort((a, b) => {
        switch (postFilters.sortBy) {
          case "oldest":
            return comparePublishedValues(a, b);
          case "a-z":
            return String(a.title || a.name || a.id || "").localeCompare(String(b.title || b.name || b.id || ""));
          case "z-a":
            return String(b.title || b.name || b.id || "").localeCompare(String(a.title || a.name || a.id || ""));
          case "latest":
          default:
            return comparePublishedValues(b, a);
        }
      });

      return sorted;
    }

    function getPostTypeCounts(posts) {
      const counts = {
        images: 0,
        videos: 0,
        audio: 0,
        documents: 0,
        archives: 0,
        other: 0
      };

      for (const post of posts) {
        const types = getPostContentTypes(post);
        for (const type of types) {
          counts[type] += 1;
        }
      }

      return counts;
    }

    function getPostContentTypes(post) {
      const types = new Set();

      if ((post.images || []).length || (post.attachments || []).some((attachment) => isDisplayableImageAttachment(attachment))) {
        types.add("images");
      }

      if (
        post.video ||
        post.videoPreview ||
        (post.attachments || []).some((attachment) => getAttachmentMimeType(attachment).startsWith("video/"))
      ) {
        types.add("videos");
      }

      if (
        post.audio ||
        post.audioPreview ||
        (post.attachments || []).some((attachment) => getAttachmentMimeType(attachment).startsWith("audio/"))
      ) {
        types.add("audio");
      }

      for (const attachment of post.attachments || []) {
        if (!attachment) {
          continue;
        }

        const category = categorizeAttachment(attachment);
        if (category) {
          types.add(category);
        }
      }

      return types;
    }

    function categorizeAttachment(attachment) {
      const mimeType = getAttachmentMimeType(attachment);
      const name = getAttachmentName(attachment).toLowerCase();

      if (isDisplayableImageAttachment(attachment)) {
        return "images";
      }

      if (mimeType.startsWith("video/")) {
        return "videos";
      }

      if (mimeType.startsWith("audio/")) {
        return "audio";
      }

      if (
        mimeType === "application/pdf" ||
        mimeType.startsWith("text/") ||
        mimeType.includes("word") ||
        mimeType.includes("excel") ||
        mimeType.includes("powerpoint") ||
        mimeType.includes("officedocument") ||
        /\.(pdf|txt|rtf|doc|docx|xls|xlsx|ppt|pptx|odt|ods|odp)$/i.test(name)
      ) {
        return "documents";
      }

      if (
        mimeType.includes("zip") ||
        mimeType.includes("rar") ||
        mimeType.includes("7z") ||
        mimeType.includes("tar") ||
        mimeType.includes("gzip") ||
        /\.(zip|rar|7z|tar|gz|bz2|xz)$/i.test(name)
      ) {
        return "archives";
      }

      return attachment.id ? "other" : "";
    }

    function isDisplayableImageAttachment(attachment) {
      const mimeType = getAttachmentMimeType(attachment);
      const name = getAttachmentName(attachment).toLowerCase();

      if (!mimeType.startsWith("image/")) {
        return false;
      }

      if (/\.(psd|psb|xcf|kra|clip|afphoto|pdn)$/i.test(name)) {
        return false;
      }

      if (
        mimeType.includes("photoshop") ||
        mimeType.includes("x-xcf") ||
        mimeType.includes("krita") ||
        mimeType.includes("clip-studio") ||
        mimeType.includes("paint.net")
      ) {
        return false;
      }

      return true;
    }

    function getAttachmentMimeType(attachment) {
      return String(
        attachment?.mimeType ||
        attachment?.mimetype ||
        attachment?.mime_type ||
        attachment?.contentType ||
        attachment?.content_type ||
        ""
      ).toLowerCase();
    }

    function getAttachmentName(attachment) {
      return String(
        attachment?.name ||
        attachment?.fileName ||
        attachment?.file_name ||
        attachment?.filename ||
        attachment?.downloadName ||
        ""
      );
    }

    function getMediaAssetName(asset) {
      return String(
        asset?.name ||
        asset?.fileName ||
        asset?.file_name ||
        asset?.filename ||
        asset?.downloadName ||
        ""
      );
    }

    function comparePublishedValues(leftPost, rightPost) {
      const a = getComparableTimestamp(leftPost);
      const b = getComparableTimestamp(rightPost);
      return a - b;
    }

    function getComparableTimestamp(post) {
      const numeric = Number(post?.publishedAt);
      if (!Number.isNaN(numeric) && numeric > 0) {
        return numeric;
      }

      const parsed = Date.parse(String(post?.publishedAt || ""));
      return Number.isNaN(parsed) ? 0 : parsed;
    }

    function renderPostPreviewMedia(items) {
      if (!items.length) {
        return "";
      }

      if (items.length === 1) {
        const item = items[0];
        const mediaHtml = item.type === "video"
          ? `<video class="post-cover" src="${escapeAttr(item.previewSrc || item.src)}" muted playsinline preload="metadata" data-preview-index="0"></video>`
          : `<img class="post-cover" src="${escapeAttr(item.previewSrc || item.src)}" alt="" data-preview-index="0">`;
        return mediaHtml;
      }

      const visibleItems = items.slice(0, 4);
      const gridClass = visibleItems.length === 2
        ? "count-2"
        : visibleItems.length === 3
          ? "count-3"
          : "count-4plus";

      const tiles = visibleItems.map((item, index) => {
        const mediaHtml = item.type === "video"
          ? `<video src="${escapeAttr(item.previewSrc || item.src)}" muted playsinline preload="metadata"></video>`
          : `<img src="${escapeAttr(item.previewSrc || item.src)}" alt="">`;
        const overflow = index === 3 && items.length > 4
          ? `<div class="post-cover-more">+${items.length - 4}</div>`
          : "";

        return `
          <div class="post-cover-tile" data-preview-index="${index}">
            ${mediaHtml}
            ${overflow}
          </div>
        `;
      }).join("");

      return `<div class="post-cover-grid ${gridClass}">${tiles}</div>`;
    }

    function getCampaignAvatarSrc(campaign) {
      const avatarId = campaign?.avatarImage?.id || campaign?.avatar_image?.id;
      return avatarId ? `/media/${avatarId}?t=1` : "";
    }

    function collectPostImageAssets(post) {
      const items = [];
      const seen = new Set();

      const pushAsset = (asset, label, thumbnail = false) => {
        if (!asset?.id || seen.has(asset.id)) {
          return;
        }
        seen.add(asset.id);
        items.push({
          id: asset.id,
          label,
          src: buildMediaUrl(asset.id, { thumbnail }),
          previewSrc: buildMediaUrl(asset.id, { thumbnail }),
          downloadSrc: buildMediaUrl(asset.id, { download: true, name: label }),
          type: "image"
        });
      };

      for (const image of post.images || []) {
        pushAsset(image, getMediaAssetName(image) || "Image");
      }

      for (const attachment of post.attachments || []) {
        if (attachment?.id && isDisplayableImageAttachment(attachment)) {
          pushAsset(attachment, getMediaAssetName(attachment) || "Image attachment");
        }
      }

      return items;
    }

    function collectPostRichMediaAssets(post) {
      const items = [];
      const seen = new Set();
      const candidates = [
        { asset: post.video, type: "video", label: "Video" },
        { asset: post.videoPreview, type: "video", label: "Video preview" },
        { asset: post.audio, type: "audio", label: "Audio" },
        { asset: post.audioPreview, type: "audio", label: "Audio preview" }
      ];

      for (const candidate of candidates) {
        if (!candidate.asset?.id || seen.has(candidate.asset.id)) {
          continue;
        }

        seen.add(candidate.asset.id);
        const label = getMediaAssetName(candidate.asset) || candidate.label;
        items.push({
          id: candidate.asset.id,
          label,
          src: buildMediaUrl(candidate.asset.id),
          previewSrc: candidate.type === "video" && post.videoPreview?.id
            ? buildMediaUrl(post.videoPreview.id)
            : candidate.type === "audio" && post.audioPreview?.id
              ? buildMediaUrl(post.audioPreview.id)
              : buildMediaUrl(candidate.asset.id),
          downloadSrc: buildMediaUrl(candidate.asset.id, { download: true, name: label }),
          type: candidate.type
        });
      }

      return items;
    }

    function collectPostAttachments(post) {
      const items = [];
      const seen = new Set();

      for (const attachment of post.attachments || []) {
        if (!attachment?.id || seen.has(attachment.id) || isDisplayableImageAttachment(attachment)) {
          continue;
        }

        seen.add(attachment.id);
        const name = getAttachmentName(attachment) || `Attachment ${attachment.id}`;
        const mimeType = getAttachmentMimeType(attachment) || "Unknown type";
        items.push({
          id: attachment.id,
          name,
          mimeType,
          href: buildMediaUrl(attachment.id, { download: true, name })
        });
      }

      return items;
    }

    function renderPostAssetSection(title, items, groupName) {
      if (!items.length) {
        return "";
      }

      const cards = items.map((item, index) => {
        const mediaHtml = item.type === "video"
          ? `<video preload="metadata" src="${escapeAttr(item.src)}" data-viewer-index="${index}"></video>`
          : item.type === "audio"
            ? `<audio controls preload="metadata" src="${escapeAttr(item.src)}"></audio>`
            : `<img src="${escapeAttr(item.src)}" alt="" data-viewer-index="${index}">`;

        return `
          <div class="asset-card">
            ${mediaHtml}
            <div class="asset-body">${escapeHtml(item.label)}</div>
          </div>
        `;
      }).join("");

      return `
        <section class="detail-section" data-viewer-group="${escapeAttr(groupName)}">
          <h3>${escapeHtml(title)}</h3>
          <div class="asset-grid">${cards}</div>
        </section>
      `;
    }

    function renderPostAttachmentSection(items) {
      if (!items.length) {
        return "";
      }

      const rows = items.map((item) => `
        <div class="attachment-item">
          <div>
            <div class="attachment-name">${escapeHtml(item.name)}</div>
            <div class="muted" style="margin-top:0.25rem;">${escapeHtml(item.mimeType)}</div>
          </div>
          <a class="action-link" href="${escapeAttr(item.href)}" target="_blank" rel="noreferrer">Open</a>
        </div>
      `).join("");

      return `
        <section class="detail-section">
          <h3>Attachments</h3>
          <div class="attachment-list">${rows}</div>
        </section>
      `;
    }

    function bindViewerSection(groupName, items) {
      if (!items.length) {
        return;
      }

      for (const node of detailsEl.querySelectorAll(`[data-viewer-group="${groupName}"] [data-viewer-index]`)) {
        const index = Number(node.dataset.viewerIndex);
        if (Number.isNaN(index)) {
          continue;
        }

        node.addEventListener("click", (event) => {
          event.preventDefault();
          openViewer(items, index);
        });
      }
    }

    function wireViewer() {
      viewerCloseEl.onclick = () => closeViewer();
      viewerPrevEl.onclick = () => showViewerItem(viewerIndex - 1);
      viewerNextEl.onclick = () => showViewerItem(viewerIndex + 1);

      viewerOverlayEl.onclick = (event) => {
        if (event.target === viewerOverlayEl || event.target === viewerStageEl) {
          closeViewer();
        }
      };

      document.addEventListener("keydown", (event) => {
        if (!viewerOverlayEl.classList.contains("open")) {
          return;
        }

        if (event.key === "Escape") {
          closeViewer();
        } else if (event.key === "ArrowLeft") {
          showViewerItem(viewerIndex - 1);
        } else if (event.key === "ArrowRight") {
          showViewerItem(viewerIndex + 1);
        }
      });

      viewerStageEl.addEventListener("touchstart", (event) => {
        if (!viewerOverlayEl.classList.contains("open") || event.touches.length !== 1) {
          viewerTouchActive = false;
          return;
        }

        viewerTouchActive = true;
        viewerTouchStartX = event.touches[0].clientX;
        viewerTouchStartY = event.touches[0].clientY;
      }, { passive: true });

      viewerStageEl.addEventListener("touchend", (event) => {
        if (!viewerTouchActive || !viewerOverlayEl.classList.contains("open")) {
          return;
        }

        viewerTouchActive = false;
        const touch = event.changedTouches[0];
        if (!touch) {
          return;
        }

        const deltaX = touch.clientX - viewerTouchStartX;
        const deltaY = touch.clientY - viewerTouchStartY;
        if (Math.abs(deltaX) < 50 || Math.abs(deltaY) > 70) {
          return;
        }

        if (deltaX < 0) {
          showViewerItem(viewerIndex + 1);
        } else {
          showViewerItem(viewerIndex - 1);
        }
      }, { passive: true });

      viewerStageEl.addEventListener("touchcancel", () => {
        viewerTouchActive = false;
      }, { passive: true });
    }

    function openViewer(items, index) {
      if (!items.length) {
        return;
      }

      viewerItems = items;
      viewerOverlayEl.classList.add("open");
      document.body.style.overflow = "hidden";
      showViewerItem(index);
    }

    function closeViewer() {
      viewerOverlayEl.classList.remove("open");
      viewerStageEl.innerHTML = "";
      viewerItems = [];
      document.body.style.overflow = "";
    }

    function showViewerItem(index) {
      if (!viewerItems.length) {
        return;
      }

      viewerIndex = (index + viewerItems.length) % viewerItems.length;
      const item = viewerItems[viewerIndex];
      viewerStageEl.innerHTML = "";

      const node = item.type === "video"
        ? document.createElement("video")
        : document.createElement("img");

      node.className = "viewer-media";
      node.src = item.src;

      if (item.type === "video") {
        node.controls = true;
        node.autoplay = true;
      } else {
        node.alt = item.label || "";
      }

      viewerStageEl.appendChild(node);
      viewerCounterEl.textContent = `${viewerIndex + 1} / ${viewerItems.length}`;
      viewerCaptionEl.textContent = item.label || "";
      viewerDownloadEl.href = item.downloadSrc || appendDownloadFlag(item.src);
      viewerDownloadEl.removeAttribute("download");
      viewerPrevEl.hidden = viewerItems.length < 2;
      viewerNextEl.hidden = viewerItems.length < 2;
    }

    function appendDownloadFlag(url) {
      return url.includes("?") ? `${url}&dl=1` : `${url}?dl=1`;
    }

    function buildMediaUrl(id, options = {}) {
      const params = new URLSearchParams();
      if (options.thumbnail) {
        params.set("t", "1");
      }
      if (options.download) {
        params.set("dl", "1");
      }
      if (options.name) {
        params.set("name", options.name);
      }
      const query = params.toString();
      return `/media/${encodeURIComponent(id)}${query ? `?${query}` : ""}`;
    }

    function getCollectionIconSrc(collection) {
      const iconId =
        collection?.coverImage?.id ||
        collection?.cover_image?.id ||
        collection?.image?.id ||
        collection?.thumbnail?.id ||
        collection?.thumb?.id;
      return iconId ? `/media/${iconId}?t=1` : "";
    }

    function getInitials(value) {
      return String(value || "")
        .split(/\s+/)
        .filter(Boolean)
        .slice(0, 2)
        .map((part) => part[0] || "")
        .join("") || "?";
    }

    function getCampaignCoverSrc(campaign) {
      const coverId = campaign?.coverPhoto?.id || campaign?.cover_photo?.id;
      return coverId ? `/media/${coverId}` : "";
    }

    function getCampaignSummary(campaign) {
      const summary = sanitizeText(
        campaign?.summary ||
        campaign?.description ||
        campaign?.creator?.summary ||
        campaign?.creator?.about ||
        campaign?.creator?.fullName ||
        "Downloaded Patreon campaign"
      );

      return summary || "Downloaded Patreon campaign";
    }

    function sanitizeText(value) {
      return decodeHtmlEntities(
        String(value || "")
          .replace(/<style[\s\S]*?<\/style>/gi, " ")
          .replace(/<script[\s\S]*?<\/script>/gi, " ")
          .replace(/<br\s*\/?>/gi, "\n")
          .replace(/<\/p>/gi, "\n\n")
          .replace(/<\/li>/gi, "\n")
          .replace(/<[^>]+>/g, " ")
      )
        .replace(/\r/g, "")
        .replace(/[ \t]+\n/g, "\n")
        .replace(/\n{3,}/g, "\n\n")
        .replace(/[ \t]{2,}/g, " ")
        .trim();
    }

    function decodeHtmlEntities(value) {
      const textarea = document.createElement("textarea");
      textarea.innerHTML = value;
      return textarea.value;
    }

    function getPostPreviewSrc(post) {
      const imageId = post?.images?.[0]?.id;
      if (imageId) {
        return `/media/${imageId}`;
      }

      const coverId = post?.coverImage?.id || post?.thumbnail?.id || post?.videoPreview?.id;
      return coverId ? `/media/${coverId}` : "";
    }

    function wireTabs() {
      tabsEl.forEach((tab) => {
        tab.addEventListener("click", async () => {
          currentTab = tab.dataset.tab;
          currentView = "browse";
          browseState = {
            kind: "campaign-tab",
            campaignId: activeCampaignId,
            tab: currentTab,
            page: currentTab === "posts" ? currentPostPage : 1
          };
          setHeroVisible(true);
          updateTabs();
          await renderCurrentTab();
        });
      });
    }

    function wireSidebarMenu() {
      if (menuButtonEl) {
        menuButtonEl.onclick = () => {
          document.body.classList.toggle("sidebar-open");
        };
      }

      if (sidebarScrimEl) {
        sidebarScrimEl.onclick = () => {
          document.body.classList.remove("sidebar-open");
        };
      }

      window.addEventListener("resize", () => {
        if (window.innerWidth > 980) {
          document.body.classList.remove("sidebar-open");
        }
      });
    }

    function wireAuthUi() {
      adminButtonEl.onclick = async () => {
        adminReturnState = captureBrowseState();
        await renderAdminPage();
      };

      logoutButtonEl.onclick = async () => {
        await postJson("/api/auth/logout", {});
        activeCampaign = null;
        activeCampaignId = null;
        await loadRuntime();
      };
    }

    function wirePostFilterOverlay() {
      closePostFilterEl.onclick = () => closePostFilterOverlay();
      postFilterOverlayEl.onclick = (event) => {
        if (event.target === postFilterOverlayEl) {
          closePostFilterOverlay();
        }
      };
    }

    function openPostFilterOverlay(campaignId, totalPosts, draftFilters = clonePostFilters(postFilters)) {
      const counts = getPostTypeCounts(cachedPosts);
      const contentTypeOptions = [
        ["images", "Images"],
        ["videos", "Videos"],
        ["audio", "Audio"],
        ["documents", "Documents"],
        ["archives", "Compressed archives"],
        ["other", "Other files"]
      ];

      postFilterBodyEl.innerHTML = `
        <div class="filter-section">
          <h3>Content type</h3>
          <div class="filter-chip-grid">
            ${contentTypeOptions.map(([key, label]) => `
              <button type="button" class="filter-chip ${draftFilters.contentTypes.includes(key) ? "active" : ""}" data-filter-chip="${key}">
                ${escapeHtml(label)} (${counts[key] || 0})
              </button>
            `).join("")}
          </div>
        </div>
        <div class="filter-section">
          <h3>Post access</h3>
          <label class="filter-choice">
            <input type="radio" name="post-access" value="accessible" ${draftFilters.access === "accessible" ? "checked" : ""}>
            Posts you have access to
          </label>
          <label class="filter-choice">
            <input type="radio" name="post-access" value="all" ${draftFilters.access === "all" ? "checked" : ""}>
            All posts
          </label>
        </div>
        <div class="filter-section">
          <h3>Sort by</h3>
          <label class="filter-choice"><input type="radio" name="post-sort" value="latest" ${draftFilters.sortBy === "latest" ? "checked" : ""}>Latest</label>
          <label class="filter-choice"><input type="radio" name="post-sort" value="oldest" ${draftFilters.sortBy === "oldest" ? "checked" : ""}>Oldest</label>
          <label class="filter-choice"><input type="radio" name="post-sort" value="a-z" ${draftFilters.sortBy === "a-z" ? "checked" : ""}>A-Z</label>
          <label class="filter-choice"><input type="radio" name="post-sort" value="z-a" ${draftFilters.sortBy === "z-a" ? "checked" : ""}>Z-A</label>
        </div>
        <div class="filter-footer">
          <button type="button" class="secondary-button" id="post-filter-clear">Clear all</button>
          <button type="button" class="primary-button" id="post-filter-apply">Apply</button>
        </div>
      `;

      for (const chip of postFilterBodyEl.querySelectorAll("[data-filter-chip]")) {
        chip.onclick = () => {
          const key = chip.getAttribute("data-filter-chip");
          if (draftFilters.contentTypes.includes(key)) {
            draftFilters.contentTypes = draftFilters.contentTypes.filter((value) => value !== key);
          } else {
            draftFilters.contentTypes = [...draftFilters.contentTypes, key];
          }
          openPostFilterOverlay(campaignId, totalPosts, draftFilters);
        };
      }

      document.getElementById("post-filter-clear").onclick = () => {
        openPostFilterOverlay(campaignId, totalPosts, createDefaultPostFilters(postFilters.search));
      };

      document.getElementById("post-filter-apply").onclick = () => {
        const selectedAccess = postFilterBodyEl.querySelector('input[name="post-access"]:checked');
        const selectedSort = postFilterBodyEl.querySelector('input[name="post-sort"]:checked');
        postFilters = {
          ...clonePostFilters(draftFilters),
          search: postFilters.search,
          access: selectedAccess?.value || draftFilters.access || "all",
          sortBy: selectedSort?.value || draftFilters.sortBy || "latest"
        };
        currentPostPage = 1;
        closePostFilterOverlay();
        renderPostsList(campaignId, totalPosts);
      };

      postFilterOverlayEl.classList.add("open");
    }

    function closePostFilterOverlay() {
      postFilterOverlayEl.classList.remove("open");
    }

    async function renderAdminPage(message = "") {
      currentView = "admin";
      setHeroVisible(false);
      const [usersData, invitesData] = await Promise.all([
        getJson("/api/admin/users"),
        getJson("/api/admin/invites")
      ]);

      const users = usersData.users || [];
      const invites = invitesData.invites || [];

      detailsEl.innerHTML = `
        <div class="content-inner admin-shell">
          ${message ? `<div class="message success">${escapeHtml(message)}</div>` : ""}
          <div class="admin-card">
            <div class="admin-inline">
              <div>
                <h2>Admin</h2>
                <div class="auth-note">Manage accounts, invites, and administrative access for this server.</div>
              </div>
              <button type="button" class="secondary-button" id="admin-back-browse">Back to browsing</button>
            </div>
          </div>
          <div class="admin-grid">
            <section class="admin-card">
              <h3>Create user</h3>
              <form id="admin-create-user-form" class="stack">
                <label>
                  Username
                  <input type="text" name="userName" required>
                </label>
                <label>
                  Password
                  <input type="password" name="password" minlength="8" pattern="\\S+" title="Password cannot contain spaces." required>
                </label>
                <label style="display:flex; gap:0.65rem; align-items:center;">
                  <input type="checkbox" name="isAdmin" style="width:auto;">
                  Create as admin
                </label>
                <button type="submit" class="primary-button">Create user</button>
              </form>
            </section>
            <section class="admin-card">
              <h3>Create invite</h3>
              <form id="admin-create-invite-form" class="stack">
                <label>
                  Expires in days
                  <input type="number" name="expiresInDays" min="1" placeholder="Leave blank for no expiry">
                </label>
                <button type="submit" class="primary-button">Create invite link</button>
              </form>
            </section>
          </div>
          <section class="admin-card">
            <h3>Users</h3>
            <div class="admin-list">
              ${users.map((user) => `
                <div class="admin-list-item">
                  <div class="admin-list-header">
                    <div>
                      <strong>${escapeHtml(user.userName)}</strong>
                    </div>
                    ${user.isAdmin ? '<span class="pill">Admin</span>' : ""}
                  </div>
                  <form class="admin-inline" data-user-id="${user.id}">
                    <label style="display:flex; gap:0.5rem; align-items:center; font-weight:500;">
                      <input type="checkbox" name="isAdmin" style="width:auto;" ${user.isAdmin ? "checked" : ""}>
                      Admin
                    </label>
                    <button type="submit" class="secondary-button">Save</button>
                    <button type="button" class="secondary-button" data-delete-user="${user.id}">Remove</button>
                  </form>
                </div>
              `).join("")}
            </div>
          </section>
          <section class="admin-card">
            <h3>Invite links</h3>
            <div class="admin-list">
              ${invites.length ? invites.map((invite) => `
                <div class="admin-list-item">
                  <div class="admin-list-header">
                    <div>
                      <strong>${escapeHtml(invite.inviteUrl)}</strong>
                      <div class="muted" style="margin-top:0.25rem;">Created ${escapeHtml(invite.createdAt)}</div>
                    </div>
                    ${invite.isRevoked ? '<span class="pill">Revoked</span>' : invite.isUsed ? '<span class="pill">Used</span>' : '<span class="pill">Active</span>'}
                  </div>
                  <div class="muted">Expires: ${escapeHtml(invite.expiresAt || "No expiry")} • Created by ${escapeHtml(invite.createdByUserName || "Unknown")} ${invite.usedByUserName ? `• Used by ${escapeHtml(invite.usedByUserName)}` : ""}</div>
                  <div class="admin-inline">
                    <input type="text" value="${escapeAttr(invite.inviteUrl)}" readonly>
                    <button type="button" class="secondary-button" data-copy-invite="${escapeAttr(invite.inviteUrl)}">Copy</button>
                    ${invite.isRevoked || invite.isUsed ? "" : `<button type="button" class="secondary-button" data-revoke-invite="${escapeAttr(invite.token)}">Revoke</button>`}
                    <button type="button" class="secondary-button danger-button" data-delete-invite="${escapeAttr(invite.token)}">Delete</button>
                  </div>
                </div>
              `).join("") : '<div class="empty-state">No invite links created yet.</div>'}
            </div>
          </section>
        </div>
      `;

      document.getElementById("admin-back-browse").onclick = async () => {
        await restoreFromAdmin();
      };

      document.getElementById("admin-create-user-form").onsubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        try {
          await postJson("/api/admin/users", {
            userName: String(formData.get("userName") || ""),
            password: String(formData.get("password") || ""),
            isAdmin: formData.get("isAdmin") === "on"
          });
          await renderAdminPage("User created.");
        } catch (error) {
          await renderAdminPage(error.message);
        }
      };

      document.getElementById("admin-create-invite-form").onsubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const rawDays = String(formData.get("expiresInDays") || "").trim();
        try {
          await postJson("/api/admin/invites", {
            expiresInDays: rawDays ? Number(rawDays) : null
          });
          await renderAdminPage("Invite created.");
        } catch (error) {
          await renderAdminPage(error.message);
        }
      };

      for (const form of detailsEl.querySelectorAll("form[data-user-id]")) {
        form.onsubmit = async (event) => {
          event.preventDefault();
          const formData = new FormData(form);
          const userId = form.dataset.userId;
          try {
            await fetch(`/api/admin/users/${userId}`, {
              method: "PUT",
              headers: { "Content-Type": "application/json" },
              body: JSON.stringify({
                isAdmin: formData.get("isAdmin") === "on"
              })
            }).then(assertOkJson);
            await renderAdminPage("User updated.");
          } catch (error) {
            await renderAdminPage(error.message);
          }
        };
      }

      for (const button of detailsEl.querySelectorAll("[data-delete-user]")) {
        button.onclick = async () => {
          const userId = button.getAttribute("data-delete-user");
          if (!confirm("Remove this user from the site?")) {
            return;
          }
          try {
            await fetch(`/api/admin/users/${userId}`, { method: "DELETE" }).then(assertOkJson);
            await renderAdminPage("User removed.");
          } catch (error) {
            await renderAdminPage(error.message);
          }
        };
      }

      for (const button of detailsEl.querySelectorAll("[data-revoke-invite]")) {
        button.onclick = async () => {
          const token = button.getAttribute("data-revoke-invite");
          try {
            await fetch(`/api/admin/invites/${encodeURIComponent(token)}`, { method: "DELETE" }).then(assertOkJson);
            await renderAdminPage("Invite revoked.");
          } catch (error) {
            await renderAdminPage(error.message);
          }
        };
      }

      for (const button of detailsEl.querySelectorAll("[data-delete-invite]")) {
        button.onclick = async () => {
          const token = button.getAttribute("data-delete-invite");
          if (!confirm("Delete this invite permanently?")) {
            return;
          }
          try {
            await fetch(`/api/admin/invites/${encodeURIComponent(token)}/hard`, { method: "DELETE" }).then(assertOkJson);
            await renderAdminPage("Invite deleted.");
          } catch (error) {
            await renderAdminPage(error.message);
          }
        };
      }

      for (const button of detailsEl.querySelectorAll("[data-copy-invite]")) {
        button.onclick = async () => {
          const url = button.getAttribute("data-copy-invite");
          try {
            await navigator.clipboard.writeText(url);
            await renderAdminPage("Invite link copied.");
          } catch (_) {
            await renderAdminPage("Copy failed. You can still copy the invite manually.");
          }
        };
      }
    }

    function captureBrowseState() {
      if (currentView !== "browse") {
        return activeCampaignId
          ? { kind: "campaign-tab", campaignId: activeCampaignId, tab: currentTab, page: currentPostPage }
          : { kind: "campaign-list" };
      }

      if (browseState?.kind) {
        return { ...browseState };
      }

      return activeCampaignId
        ? { kind: "campaign-tab", campaignId: activeCampaignId, tab: currentTab, page: currentPostPage }
        : { kind: "campaign-list" };
    }

    async function restoreFromAdmin() {
      const state = adminReturnState;
      adminReturnState = null;
      currentView = "browse";
      setHeroVisible(true);

      if (!state || state.kind === "campaign-list") {
        activeCampaignId = null;
        activeCampaign = null;
        await loadCampaigns();
        return;
      }

      if (state.campaignId && state.campaignId !== activeCampaignId) {
        await loadCampaign(state.campaignId);
      }

      if (!activeCampaignId || !activeCampaign) {
        await loadCampaigns();
        return;
      }

      if (state.kind === "post-detail" && state.postId) {
        currentTab = "posts";
        currentPostPage = state.page || 1;
        updateTabs();
        await loadPost(state.postId, activeCampaignId);
        return;
      }

      if (state.kind === "collection-detail" && state.collectionId) {
        currentTab = "collections";
        updateTabs();
        await loadCollection(state.collectionId, activeCampaignId, state.page || 1);
        return;
      }

      currentTab = state.tab || "posts";
      currentPostPage = state.page || 1;
      browseState = {
        kind: "campaign-tab",
        campaignId: activeCampaignId,
        tab: currentTab,
        page: currentTab === "posts" ? currentPostPage : 1
      };
      updateTabs();
      await renderCurrentTab();
    }

    function updateTabs() {
      tabsEl.forEach((tab) => {
        tab.classList.toggle("active", tab.dataset.tab === currentTab);
      });
    }

    function wireSettingsOverlay() {
      const openSettingsFromUi = (event) => {
        event.preventDefault();
        renderSetup(
          "Server settings",
          "Update the port, data directory, or writable storage. Port changes require a server restart.",
          false
        );
        openSettings();
      };

      topSettingsButtonEl.onclick = openSettingsFromUi;

      closeSettingsEl.onclick = () => {
        if (!runtimeInfo || !runtimeInfo.setupRequired) {
          closeSettings();
        }
      };

      settingsOverlayEl.onclick = (event) => {
        if (event.target === settingsOverlayEl && runtimeInfo && !runtimeInfo.setupRequired) {
          closeSettings();
        }
      };
    }

    function openSettings() {
      settingsOverlayEl.classList.add("open");
    }

    function closeSettings() {
      settingsOverlayEl.classList.remove("open");
    }

    function initializeTheme() {
      let theme = "dark";

      try {
        const savedTheme = localStorage.getItem("patreon-dl-server-theme");
        if (savedTheme === "dark" || savedTheme === "light") {
          theme = savedTheme;
        }
      } catch (_) {
      }

      setTheme(theme);
      if (themeToggleEl) {
        themeToggleEl.addEventListener("click", () => {
          setTheme(rootEl.dataset.theme === "dark" ? "light" : "dark");
        });
      }
    }

    function setTheme(theme) {
      rootEl.dataset.theme = theme;
      if (themeToggleEl) {
        themeToggleEl.textContent =
          theme === "dark" ? "Switch to Light Mode" : "Switch to Dark Mode";
      }

      try {
        localStorage.setItem("patreon-dl-server-theme", theme);
      } catch (_) {
      }
    }

    function escapeAttr(value) {
      return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("\"", "&quot;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;");
    }

    function escapeHtml(value) {
      return escapeAttr(value).replaceAll("'", "&#39;");
    }

    loadRuntime().catch((error) => {
      renderHero(null);
      detailsEl.innerHTML = `
        <div class="message error">
          <strong>Server error</strong>
          <div class="muted" style="margin-top:0.4rem;">${escapeHtml(error.message)}</div>
        </div>
      `;
    });
  </script>
</body>
</html>
""";
}
