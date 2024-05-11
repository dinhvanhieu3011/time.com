import asyncio
import json
import os
import sys
from TikTokApi import TikTokApi

async def user_example(username, ms_token):
    user_videos_data = {}
    ms_token = os.environ.get(
    "ms_token", ms_token
)
    async with TikTokApi() as api:
        context_options = {
            'viewport': {'width': 1280, 'height': 1024},
            'user_agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36'
        }
        await api.create_sessions(ms_tokens=[ms_token], num_sessions=1, sleep_after=3, context_options=context_options)
        user = api.user(username)
        user_data = await user.info()
        user_videos_data['user_data'] = user_data
        videosOfUser = user.videos(count=7)

        videosWithData = []
        async for video in videosOfUser:
            videosWithData.append(video.as_dict)

        
        videos_data = []
        for video in videosWithData:
            videos_data.append({
                'createTime': video.__getitem__('createTime'),
                'stats': video.__getitem__('stats')
            })
        user_videos_data['videos'] = videos_data
    with open('user_videos1.json', 'w+') as json_file:
        json.dump(user_videos_data, json_file, indent=4)
    return user_videos_data
    # # Write data to JS ON file


if __name__ == "__main__":
    username = sys.argv[1]
    ms_token = sys.argv[2] #BYVpxMmBfkNuhEMVLlwxNOsaEYdxEGiMjcXAZC-xA3WSW7_MKtSmJv9MKWAeuKKt1F2iyRO5of6biGhmRaFX9QwvkxGR0XeXsHRHJ-pHiOPuW3lKTDKyo2Ig3z45ihtspWqMzW_ytSVMjg3Ai2M=
    asyncio.run(user_example(username,ms_token))
    # asyncio.run(user_example("powerseal12"))